using System.Threading.Channels;
using DataTransport;
using EventProcessor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddSingleton(new Dictionary<Guid, Channel<Event>>());
var app = builder.Build();

app.MapGrpcService<PublishSubscriptionService>();

app.Run();