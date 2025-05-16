using PlatformNext_MerchantPortal_SampleCSharp.Models;
using System.Text.Json;

namespace PlatformNext_MerchantPortal_SampleCSharp
{
    public class AuthService
    {
        public static async Task<string> GetTokenAsync(ApiSettings config)
        {
            using var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{config.ApiUrl}/connect/token");
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", config.Username),
                new KeyValuePair<string, string>("password", config.Password),
                new KeyValuePair<string, string>("client_id", config.ClientId),
                new KeyValuePair<string, string>("scope", config.Scope)
            });

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Auth failed: {response.StatusCode} {errorText}");
            }

            var content = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(content);
            var token = jsonDoc.RootElement.GetProperty("access_token").GetString();

            return token!;
        }
    }
}
