using DataTransport;

namespace IntegrationEvents;

public abstract record IntegrationEvent : IIntegrationEvent
{
    protected IntegrationEvent(Guid sourceId)
    {
        SourceId = sourceId;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public Guid SourceId { get; }
    public abstract string Name { get; }
}