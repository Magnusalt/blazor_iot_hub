namespace DeviceHub.Models;

public interface IDomainEvent
{
    Guid SourceId { get; }
}

public record DeviceStateChanged(Guid SourceId) : IDomainEvent
{
    public Guid DeviceId { get; set; }
    public DateTime ReportDate { get; set; }
    public double Value { get; set; }
    public Status Status { get; set; }
    public DeviceType DeviceType { get; set; }
    public Unit Unit { get; set; }
}

public interface ICommand
{
}

public record ExcludeSensor : ICommand
{
    public Guid DeviceId { get; set; }
}

public interface IQuery<T>
{
}

public record DeviceQuery<Device>
{
    public Guid DeviceId { get; set; }
}