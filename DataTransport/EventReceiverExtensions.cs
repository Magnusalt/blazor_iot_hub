using ApplicationInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DataTransport;

public static class EventReceiverExtensions
{
    public static IServiceCollection AddEventHandling(this IServiceCollection services, Uri eventProcessorUri,
        Guid clientId)
    {
        services.AddGrpcClient<PubSub.PubSubClient>(options => options.Address = eventProcessorUri);

        services.AddHostedService(provider =>
            new EventReceiver(provider.GetService<PubSub.PubSubClient>()!, clientId));

        services.AddTransient<IEventPublisher, EventPublisher>();
        
        return services;
    }
}