using DataTransport;
using DeviceHub;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcClient<PubSub.PubSubClient>(options => options.Address = new Uri("https://localhost:7016"));

builder.Services.AddHostedService(provider =>
    new EventReceiver(provider.GetService<PubSub.PubSubClient>()!, Guid.NewGuid()));

var app = builder.Build();

app.Run();