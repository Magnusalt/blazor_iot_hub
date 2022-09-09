namespace IntegrationEvents;

public record DeviceValueUpdated(Guid SourceId) : IntegrationEvent(SourceId)
{
    public override string Name => nameof(DeviceValueUpdated);
}