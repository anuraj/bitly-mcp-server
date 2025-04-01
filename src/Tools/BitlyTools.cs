using ModelContextProtocol.Server;

using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BitlyMcpServer.Tools
{
    [McpServerToolType]
    public class BitlyTools
    {
        [McpServerTool, Description("Shorten a long URL to bitly short URL")]
        public static async Task<string?> Shorten(HttpClient client, [Description("The long URL to shorten")] string longUrl)
        {
            var apiKey = Environment.GetEnvironmentVariable("BITLY_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("The BITLY_API_KEY environment variable is not set.");
            }
            var request = new HttpRequestMessage(HttpMethod.Post, "v4/shorten")
            {
                Content = new StringContent(JsonSerializer.Serialize(new { long_url = longUrl }), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Failed to shorten URL: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(content);
            return result == null ?
                throw new InvalidOperationException("Failed to parse response from Bitly.") : result["link"].ToString();
        }

        [McpServerTool, Description("Expand a bitly short URL to long URL")]
        public static async Task<string?> GetLongUrl(HttpClient client, [Description("The short URL to expand")] string bitlyUrl)
        {
            //If shortUrl starts with http:// or https://, replace it with nothing
            bitlyUrl = bitlyUrl.Replace("http://", "").Replace("https://", "");

            var apiKey = Environment.GetEnvironmentVariable("BITLY_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("The BITLY_API_KEY environment variable is not set.");
            }
            var request = new HttpRequestMessage(HttpMethod.Post, "v4/expand")
            {
                Content = new StringContent(JsonSerializer.Serialize(new { bitlink_id = bitlyUrl }), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Failed to expand URL: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(content);
            return result == null ?
                throw new InvalidOperationException("Failed to parse response from Bitly.") : result["long_url"].ToString();
        }

        [McpServerTool, Description("Get the click count for a bitly short URL")]
        public static async Task<string?> GetClickCount(HttpClient client, [Description("The short URL to get the click count")] string bitlyUrl)
        {
            bitlyUrl = bitlyUrl.Replace("http://", "").Replace("https://", "");

            var apiKey = Environment.GetEnvironmentVariable("BITLY_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("The BITLY_API_KEY environment variable is not set.");
            }
            var request = new HttpRequestMessage(HttpMethod.Get, $"v4/bitlinks/{bitlyUrl}/clicks?unit=month&units=-1");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Failed to expand URL: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(content);
            return result == null ?
                throw new InvalidOperationException("Failed to parse response from Bitly.") : result["link_clicks"].ToString();
        }
    }
}
