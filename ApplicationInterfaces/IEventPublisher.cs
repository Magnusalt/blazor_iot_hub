using Models;

namespace ApplicationInterfaces;

public interface IEventPublisher
{
    Task<PublishResult> Publish(Event @event);
}