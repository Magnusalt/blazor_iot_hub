namespace DataTransport;

public interface IIntegrationEvent
{
    Guid Id { get; }
    Guid SourceId { get; }
    string Name { get; }
}