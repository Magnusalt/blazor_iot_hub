using Grpc.Core;
using Microsoft.Extensions.Hosting;

namespace DataTransport;

public class EventReceiver : BackgroundService
{
    private readonly PubSub.PubSubClient _client;
    private readonly string _clientId;

    public EventReceiver(PubSub.PubSubClient client, Guid clientId)
    {
        _client = client;
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
            var payload = e.Payload.ToByteArray();
            Console.WriteLine($"{e.SourceId} said: {payload[0]}{payload[1]}");
        }
    }
}