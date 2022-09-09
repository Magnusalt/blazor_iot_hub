using DataTransport;
using DeviceHub;
using DeviceHub.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var sourceId = new SourceId();
builder.Services.AddSingleton(sourceId);
builder.Services.AddEventHandling(new Uri("https://localhost:7016"), sourceId.Value);
builder.Services.AddSingleton<DeviceHub.Models.DeviceHub>();

var app = builder.Build();

app.MapGet("/devices",
    ([FromServices] DeviceHub.Models.DeviceHub hub) => hub.Devices.Values);

app.MapPost("/devices",
    ([FromServices]DeviceHub.Models.DeviceHub hub, DeviceReport report) =>
    {
        ;
        hub.When(new DeviceStateChanged(report.DeviceId)
        {
            Status = report.Status,
            Unit = report.Unit,
            Value = report.Value,
            DeviceType = report.DeviceType,
            DeviceId = report.DeviceId,
            ReportDate = report.DeviceTime
        });
    });

app.Run();