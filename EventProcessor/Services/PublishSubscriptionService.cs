using System.Threading.Channels;
using DataTransport;
using Google.Protobuf;
using Grpc.Core;
using Event = Models.Event;

namespace EventProcessor.Services;

public class PublishSubscriptionService : PubSub.PubSubBase
{
    private readonly ILogger<PublishSubscriptionService> _logger;
    private readonly Dictionary<Guid, Channel<Event>> _subscribers;

    public PublishSubscriptionService(ILogger<PublishSubscriptionService> logger)
    {
        _logger = logger;
        _subscribers = new Dictionary<Guid, Channel<Event>>();
    }

    public override async Task Subscribe(Subscriber request, IServerStreamWriter<DataTransport.Event> responseStream,
        ServerCallContext context)
    {
        var readerChannel = Channel.CreateUnbounded<Event>();
        _subscribers.Add(Guid.Parse(request.Id), readerChannel);

        await foreach (var e in readerChannel.Reader.ReadAllAsync())
        {
            await responseStream.WriteAsync(new DataTransport.Event
                { Id = e.Id.ToString(), Payload = ByteString.CopyFrom(e.Payload), SourceId = e.SourceId.ToString() });
        }
    }

    public override Task<Unsubscription> Unsubscribe(Subscriber request, ServerCallContext context)
    {
        var id = Guid.Parse(request.Id);
        if (_subscribers.TryGetValue(id, out var channel))
        {
            channel.Writer.Complete();
            _subscribers.Remove(id);
        }

        return Task.FromResult(new Unsubscription { Id = Guid.NewGuid().ToString() });
    }

    public override async Task<PublishResult> Publish(DataTransport.Event request, ServerCallContext context)
    {
        try
        {
            foreach (var (_, channel) in _subscribers)
            {
                await channel.Writer.WriteAsync(new Event(Guid.Parse(request.SourceId), request.Payload.ToByteArray()));
            }

            return new PublishResult { Ok = true };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to publish event {Event}", request.Id);
            return new PublishResult { Ok = false };
        }
    }
}