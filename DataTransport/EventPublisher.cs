using ApplicationInterfaces;
using Google.Protobuf;

namespace DataTransport;

public class EventPublisher : IEventPublisher
{
    private readonly PubSub.PubSubClient _client;

    public EventPublisher(PubSub.PubSubClient client)
    {
        _client = client;
    }

    public async Task<Models.PublishResult> Publish(Models.Event @event)
    {
        var result = await _client.PublishAsync(new Event
        {
            Id = Models.Event.Id.ToString(), Payload = ByteString.CopyFrom(@event.Payload),
            SourceId = @event.SourceId.ToString()
        });
        return new Models.PublishResult { Ok = result.Ok };
    }
}