namespace IntegrationEvents;

public record NewDeviceConnected(Guid SourceId) : IntegrationEvent(SourceId)
{
    public override string Name => nameof(NewDeviceConnected);
}