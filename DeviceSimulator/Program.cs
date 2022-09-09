// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text;
using System.Text.Json;

Console.WriteLine("Hello, World!");


var httpClient = new HttpClient
{
    DefaultRequestVersion = HttpVersion.Version20
};
httpClient.BaseAddress = new Uri("https://localhost:7145");

var result = await httpClient.PostAsync("/devices",
    new StringContent(JsonSerializer.Serialize(new
    {
        Value = 2.0,
        DeviceId = Guid.NewGuid(),
        ReportDate = DateTime.Now
    }), Encoding.UTF8, "application/json"));

Console.WriteLine(result.StatusCode.ToString());