namespace Models;

public abstract record Event
{
    public static Guid Id => Guid.NewGuid();
    public abstract string Name { get; }
    public Guid SourceId { get; init; }
}

public record DeviceReadingReported : Event
{
    public override string Name => nameof(DeviceReadingReported);
}
