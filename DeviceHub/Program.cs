using DataTransport;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEventHandling(new Uri("https://localhost:7016"), Guid.NewGuid());

var app = builder.Build();

app.Run();