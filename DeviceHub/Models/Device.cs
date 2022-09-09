namespace DeviceHub.Models;

public record Device
{
    public Guid Id { get; set; }
    public DeviceType DeviceType { get; init; }
    public double Value { get; init; }
    public Unit Unit { get; init; }
    public (DateTime, double)[] History { get; init; }
    public Status Status { get; set; }
    public bool IsExcluded { get; set; }

    public Device When(DeviceStateChanged stateChanged)
    {
        var history = History.Append((stateChanged.ReportDate, Value)).ToArray();
        return this with
        {
            Status = stateChanged.Status,
            Value = stateChanged.Value,
            History = history
        };
    }
}

public enum Unit
{
    Celsius
}

public enum DeviceType
{
    Thermometer
}

public enum Status
{
    Ok,
    LowBattery
}

public record DeviceReport
{
    public Guid DeviceId { get; set; }
    public DeviceType DeviceType { get; init; }
    public double Value { get; init; }
    public Unit Unit { get; init; }
    public Status Status { get; set; }
    public DateTime DeviceTime { get; set; }
}