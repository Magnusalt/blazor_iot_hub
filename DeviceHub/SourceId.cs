namespace DeviceHub;

public class SourceId
{
    public SourceId()
    {
        Value = Guid.NewGuid();
    }

    public Guid Value { get; }
}