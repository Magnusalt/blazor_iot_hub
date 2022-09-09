using System.Text.Json;
using Google.Protobuf;

namespace DataTransport;

public class EventPublisher : IEventPublisher
{
    private readonly PubSub.PubSubClient _client;

    public EventPublisher(PubSub.PubSubClient client)
    {
        _client = client;
    }

    public async Task<bool> Publish(IIntegrationEvent @event)
    {
        await using var jsonStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(jsonStream, @event);
        var result = await _client.PublishAsync(new Event
        {
            Id = @event.Id.ToString(),
            Payload = ByteString.CopyFrom(jsonStream.ToArray()),
            SourceId = @event.SourceId.ToString(),
            Name = @event.Name
        });
        return true;
    }
}