using DataTransport;
using IntegrationEvents;

namespace DeviceHub.Models;

public class DeviceHub
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IEventRelay _eventRelay;
    private readonly SourceId _sourceId;

    public DeviceHub(IEventRelay eventRelay, IEventPublisher eventPublisher, SourceId sourceId)
    {
        _eventRelay = eventRelay;
        _eventPublisher = eventPublisher;
        _sourceId = sourceId;
        _eventRelay.EventReceived += EventReceiverOnEventReceived;
        Devices = new Dictionary<Guid, Device>();
    }

    public Dictionary<Guid, Device> Devices { get; }

    private void EventReceiverOnEventReceived(object? sender, IntegrationEventArgs e)
    {
        throw new NotImplementedException();
    }

    public void Handle(ExcludeSensor excludeSensorCommand)
    {
        if (Devices.TryGetValue(excludeSensorCommand.DeviceId, out var device))
        {
            Devices[excludeSensorCommand.DeviceId] = device with { IsExcluded = true };
        }
    }

    public void When(DeviceStateChanged stateChanged)
    {
        if (Devices.TryGetValue(stateChanged.DeviceId, out var device))
        {
            Devices[stateChanged.DeviceId] = device.When(stateChanged);
        }
        else
        {
            Devices.TryAdd(stateChanged.DeviceId, new Device
            {
                Id = stateChanged.DeviceId,
                Status = stateChanged.Status,
                DeviceType = stateChanged.DeviceType,
                Unit = stateChanged.Unit,
                Value = stateChanged.Value,
                IsExcluded = true
            });
            _eventPublisher.Publish(new NewDeviceConnected(_sourceId.Value));
        }

        _eventPublisher.Publish(new DeviceValueUpdated(_sourceId.Value));
    }
}