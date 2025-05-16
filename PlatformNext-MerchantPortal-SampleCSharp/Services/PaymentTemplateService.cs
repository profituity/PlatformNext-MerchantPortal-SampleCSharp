using PlatformNext_MerchantPortal_SampleCSharp.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace PlatformNext_MerchantPortal_SampleCSharp.Services
{
    public class PaymentTemplateService
    {
        public static async Task<object> CreatePaymentTemplateAsync(PaymentTemplateDto dto, string token, string apiUrl)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var endpoint = $"{apiUrl}/api/merchant-portal/payments/paymenttemplates";
            var response = await client.PostAsJsonAsync(endpoint, dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Create Template failed: {response.StatusCode} {error}");
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<object>(result)!;
        }

        public static async Task<object> CreatePaymentFromTemplateAsync(PaymentTemplateDto templateDto, PaymentFromTemplateDto paymentFromTemplateDto, string token, string apiUrl)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Step 1: Create the template
            var templateResponse = await client.PostAsJsonAsync($"{apiUrl}/api/merchant-portal/payments/paymenttemplates", templateDto);
            if (!templateResponse.IsSuccessStatusCode)
            {
                var error = await templateResponse.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Create Template failed: {templateResponse.StatusCode} {error}");
            }

            var createdTemplate = await templateResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            if (!createdTemplate!.TryGetValue("id", out var templateIdObj) || templateIdObj is not JsonElement idElem || idElem.ValueKind != JsonValueKind.String)
            {
                throw new Exception("No payment template ID returned for create payment operation.");
            }

            var templateId = idElem.GetString();
            if (string.IsNullOrEmpty(templateId))
                throw new Exception("Payment template ID was empty or null.");

            // Step 2: Create a payment from the template
            var createPaymentUrl = $"{apiUrl}/api/merchant-portal/payments/paymentTemplates/{templateId}/createPaymentFromPaymentTemplate";
            var paymentResponse = await client.PostAsJsonAsync(createPaymentUrl, paymentFromTemplateDto);

            if (!paymentResponse.IsSuccessStatusCode)
            {
                var error = await paymentResponse.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Create Payment From Template failed: {paymentResponse.StatusCode} {error}");
            }

            var result = await paymentResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<object>(result)!;
        }

    }
}
