using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

//Display warnings and above only in console.
builder.Logging.SetMinimumLevel(LogLevel.Warning);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton(_ =>
{
    var client = new HttpClient() { BaseAddress = new Uri("https://api-ssl.bitly.com/") };
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("BitlyMcpServer", "1.0"));
    return client;
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();