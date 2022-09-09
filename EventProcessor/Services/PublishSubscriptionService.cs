using System.Threading.Channels;
using DataTransport;
using Grpc.Core;

namespace EventProcessor.Services;

public class PublishSubscriptionService : PubSub.PubSubBase
{
    private readonly ILogger<PublishSubscriptionService> _logger;
    private readonly Dictionary<Guid, Channel<Event>> _subscribers;

    public PublishSubscriptionService(ILogger<PublishSubscriptionService> logger, Dictionary<Guid, Channel<Event>> subscribers)
    {
        _logger = logger;
        _subscribers = subscribers;
    }

    public override async Task Subscribe(Subscriber request, IServerStreamWriter<Event> responseStream,
        ServerCallContext context)
    {
        var readerChannel = Channel.CreateUnbounded<Event>();
        _subscribers.Add(Guid.Parse(request.Id), readerChannel);

        await foreach (var e in readerChannel.Reader.ReadAllAsync())
        {
            await responseStream.WriteAsync(e);
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

    public override async Task<PublishResult> Publish(Event request, ServerCallContext context)
    {
        try
        {
            foreach (var (_, channel) in _subscribers.Where(pair => pair.Key != Guid.Parse(request.SourceId)))
            {
                await channel.Writer.WriteAsync(request);
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