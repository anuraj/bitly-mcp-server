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
        private static string GetApiKey()
        {
            var apiKey = Environment.GetEnvironmentVariable("BITLY_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("The BITLY_API_KEY environment variable is not set.");
            }
            return apiKey;
        }

        private static async Task<Dictionary<string, object>> SendRequest(HttpClient client, HttpRequestMessage request)
        {
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Request failed: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(content);
            if (result == null)
            {
                throw new InvalidOperationException("Failed to parse response from Bitly.");
            }
            return result;
        }

        [McpServerTool, Description("Shorten a long URL to bitly short URL")]
        public static async Task<string?> CreateBitlink(HttpClient client, [Description("The long URL to shorten")] string longUrl)
        {
            var apiKey = GetApiKey();
            var request = new HttpRequestMessage(HttpMethod.Post, "v4/shorten")
            {
                Content = new StringContent(JsonSerializer.Serialize(new { long_url = longUrl }), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var result = await SendRequest(client, request);
            return result["link"].ToString();
        }

        [McpServerTool, Description("Delete a bitly short URL")]
        public static async Task<string?> DeleteBitlink(HttpClient client, [Description("The short URL to delete")] string bitlink)
        {
            bitlink = bitlink.Replace("http://", "").Replace("https://", "");
            var apiKey = GetApiKey();
            var request = new HttpRequestMessage(HttpMethod.Delete, $"v4/bitlinks/{bitlink}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var result = await SendRequest(client, request);
            return result["link"].ToString();
        }

        [McpServerTool, Description("Update a bitly short URL")]
        public static async Task<string?> UpdateBitlink(HttpClient client, [Description("The short URL to update")] string bitlink, [Description("The new title for the short URL")] string title)
        {
            bitlink = bitlink.Replace("http://", "").Replace("https://", "");
            var apiKey = GetApiKey();
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"v4/bitlinks/{bitlink}")
            {
                Content = new StringContent(JsonSerializer.Serialize(new { title = title }), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var result = await SendRequest(client, request);
            return result["link"].ToString();
        }

        [McpServerTool, Description("Retrieve a bitly short URL by bitlink")]
        public static async Task<string?> RetrieveByBitlink(HttpClient client, [Description("The short URL to retrieve")] string bitlink)
        {
            bitlink = bitlink.Replace("http://", "").Replace("https://", "");
            var apiKey = GetApiKey();
            var request = new HttpRequestMessage(HttpMethod.Get, $"v4/bitlinks/{bitlink}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var result = await SendRequest(client, request);
            return result.ToString();
        }

        [McpServerTool, Description("Expand a bitly short URL to long URL")]
        public static async Task<string?> GetLongUrlFromBitlink(HttpClient client, [Description("The short URL to expand")] string bitlink)
        {
            bitlink = bitlink.Replace("http://", "").Replace("https://", "");

            var apiKey = GetApiKey();
            var request = new HttpRequestMessage(HttpMethod.Post, "v4/expand")
            {
                Content = new StringContent(JsonSerializer.Serialize(new { bitlink_id = bitlink }), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var result = await SendRequest(client, request);
            return result["long_url"].ToString();
        }

        [McpServerTool, Description("Get the clicks count in month for a bitly short URL")]
        public static async Task<string?> GetClickCountByMonth(HttpClient client, [Description("The short URL to get the click count")] string bitlink)
        {
            bitlink = bitlink.Replace("http://", "").Replace("https://", "");

            var apiKey = GetApiKey();
            var request = new HttpRequestMessage(HttpMethod.Get, $"v4/bitlinks/{bitlink}/clicks?unit=month&units=-1");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var result = await SendRequest(client, request);
            return result["link_clicks"].ToString();
        }
    }
}
