using PlatformNext_MerchantPortal_SampleCSharp.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace PlatformNext_MerchantPortal_SampleCSharp.Services
{
    public class PaymentService
    {
        public static async Task<object> CreatePaymentAsync(PaymentDto dto, string token, string apiUrl)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = $"{apiUrl}/api/merchant-portal/payments/payments";
            var response = await client.PostAsJsonAsync(endpoint, dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Create Payment failed: {response.StatusCode}\n{error}");
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<object>(result)!;
        }

        public static async Task<object> BulkCreatePaymentsAsync(List<PaymentDto> dtos, string token, string apiUrl)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = $"{apiUrl}/api/merchant-portal/payments/payments/bulk";
            var response = await client.PostAsJsonAsync(endpoint, dtos);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Bulk Create failed: {response.StatusCode} {error}");
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<object>(result)!;
        }

        public static async Task<object> GetPaymentsAsync(string token, string apiUrl)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = $"{apiUrl}/api/merchant-portal/payments/payments";
            var response = await client.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Get Payments failed: {response.StatusCode} {error}");
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<object>(result)!;
        }

        public static async Task<object> GetStatusDatePaymentReportAsync(string startDate, string endDate, string token, string apiUrl)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = $"{apiUrl}/api/merchant-portal/payments/reports/statusDatePaymentReport?startDate={Uri.EscapeDataString(startDate)}&endDate={Uri.EscapeDataString(endDate)}";
            var response = await client.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Report fetch failed: {response.StatusCode} {error}");
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<object>(result)!;
        }

        public static async Task<object> CancelPaymentAsync(PaymentDto dto, string token, string apiUrl)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Step 1: Create a payment
            var createResponse = await client.PostAsJsonAsync($"{apiUrl}/api/merchant-portal/payments/payments", dto);

            if (!createResponse.IsSuccessStatusCode)
            {
                var error = await createResponse.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Create for cancel failed: {createResponse.StatusCode} {error}");
            }

            var created = await createResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            if (!created!.TryGetValue("id", out var idObj) || idObj is not JsonElement idElem || idElem.ValueKind != JsonValueKind.String)
            {
                throw new Exception("No payment ID returned for cancel operation.");
            }

            var paymentId = idElem.GetString();
            if (string.IsNullOrEmpty(paymentId))
                throw new Exception("Payment ID is null or empty.");

            // Step 2: Cancel the payment
            var cancelEndpoint = $"{apiUrl}/api/merchant-portal/payments/payments/{paymentId}/cancelPayment";
            var cancelResponse = await client.PostAsync(cancelEndpoint, null);

            if (!cancelResponse.IsSuccessStatusCode)
            {
                var cancelError = await cancelResponse.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Cancel failed: {cancelResponse.StatusCode} {cancelError}");
            }

            return new
            {
                Status = (int)cancelResponse.StatusCode,
                Message = "Payment canceled successfully"
            };
        }

        public static async Task<string> GetMostRecentPaidPaymentIdAsync(string merchantId, string token, string apiUrl)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var filter = new List<object>
            {
                new object[] { "status", "=", "Paid" },
                "and",
                new object[] { "type", "=", 1 },
                "and",
                new object[] { "merchantId", "=", merchantId }
            };

            var sort = new[]
            {
                new Dictionary<string, object> { { "selector", "creationTime" }, { "desc", true } }
            };

            var query = new Dictionary<string, string>
            {
                ["filter"] = JsonSerializer.Serialize(filter),
                ["sort"] = JsonSerializer.Serialize(sort)
            };

            var queryString = new FormUrlEncodedContent(query).ReadAsStringAsync().Result;
            var endpoint = $"{apiUrl}/api/merchant-portal/payments/payments?{queryString}";

            var response = await client.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Get paid payment failed: {response.StatusCode} {error}");
            }

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            var dataArray = json.GetProperty("data");

            if (dataArray.GetArrayLength() == 0)
                throw new Exception("No paid payments found.");

            var first = dataArray[0];
            if (!first.TryGetProperty("id", out var idProp) || idProp.ValueKind != JsonValueKind.String)
                throw new Exception("Paid payment does not contain a valid ID.");

            return idProp.GetString()!;
        }

        public static async Task<object> RefundPaymentAsync(string paymentId, string token, string apiUrl)
        {
            var refundDto = new RefundDto
            {
                Amount = 1,
                ExternalId = null!,
                Description = "Simple refund",
                SendDate = DateTime.UtcNow.ToString("o"),
                IdentificationNumber = null!
            };

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = $"{apiUrl}/api/merchant-portal/payments/payments/{paymentId}/refundPayment";
            var response = await client.PostAsJsonAsync(endpoint, refundDto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Refund failed: {response.StatusCode} {error}");
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<object>(result)!;
        }



    }
}
