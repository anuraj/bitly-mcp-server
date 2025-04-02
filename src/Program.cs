using System.Net.Http.Headers;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.Configuration.AddEnvironmentVariables();

builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services.AddSingleton(_ =>
{
    var client = new HttpClient() { BaseAddress = new Uri("https://api-ssl.bitly.com/") };
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("BitlyMcpServer", "1.0"));
    return client;
});

await builder.Build().RunAsync();