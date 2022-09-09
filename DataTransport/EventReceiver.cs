using Grpc.Core;
using Microsoft.Extensions.Hosting;

namespace DataTransport;

public sealed class EventReceiver : BackgroundService
{
    private readonly PubSub.PubSubClient _client;
    private readonly string _clientId;
    private readonly IEventRelay _eventRelay;

    public EventReceiver(PubSub.PubSubClient client, Guid clientId, IEventRelay eventRelay)
    {
        _client = client;
        _eventRelay = eventRelay;
        _clientId = clientId.ToString();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("unsubscribing");
        await _client.UnsubscribeAsync(new Subscriber { Id = _clientId }, cancellationToken: cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await StartSubscribing(stoppingToken);
    }

    private async Task StartSubscribing(CancellationToken cancellationToken)
    {
        Console.WriteLine($"Client {_clientId} started to receive events");
        var subscription = _client.Subscribe(new Subscriber { Id = _clientId }, cancellationToken: cancellationToken);
        await foreach (var e in subscription.ResponseStream.ReadAllAsync(cancellationToken))
        {
            _eventRelay.RelayEvent(e);
        }
    }
}

public interface IEventRelay
{
    void RelayEvent(Event @event);
    event EventHandler<IntegrationEventArgs> EventReceived;
}

public class EventRelay : IEventRelay
{
    public void RelayEvent(Event @event)
    {
        OnEventReceived(new IntegrationEventArgs(@event));
    }

    public event EventHandler<IntegrationEventArgs>? EventReceived;

    private void OnEventReceived(IntegrationEventArgs e)
    {
        EventReceived?.Invoke(this, e);
    }
}

public class IntegrationEventArgs : EventArgs
{
    public IntegrationEventArgs(Event @event)
    {
        Event = @event;
    }

    public Event Event { get; set; }
}