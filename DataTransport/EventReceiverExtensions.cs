using Microsoft.Extensions.DependencyInjection;

namespace DataTransport;

public static class EventReceiverExtensions
{
    public static IServiceCollection AddEventHandling(this IServiceCollection services, Uri eventProcessorUri,
        Guid clientId)
    {
        services.AddGrpcClient<PubSub.PubSubClient>(options => options.Address = eventProcessorUri);

        services.AddSingleton<IEventRelay, EventRelay>();
        services.AddHostedService(provider =>
            new EventReceiver(provider.GetService<PubSub.PubSubClient>()!, clientId,
                provider.GetService<IEventRelay>()));

        services.AddTransient<IEventPublisher, EventPublisher>();

        return services;
    }
}