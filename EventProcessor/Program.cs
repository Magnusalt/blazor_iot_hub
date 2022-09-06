using EventProcessor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<PublishSubscriptionService>();
builder.Services.AddGrpc();

var app = builder.Build();
app.MapGrpcService<PublishSubscriptionService>();

app.Run();