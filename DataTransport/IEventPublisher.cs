namespace DataTransport;

public interface IEventPublisher
{
    Task<bool> Publish(IIntegrationEvent @event);
}