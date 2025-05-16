using Microsoft.Extensions.Configuration;
using PlatformNext_MerchantPortal_SampleCSharp.Models;
using PlatformNext_MerchantPortal_SampleCSharp;
using PlatformNext_MerchantPortal_SampleCSharp.Services;
using System.Text.Json;
using PlatformNext_MerchantPortal_SampleCSharp.Helpers;

await RunAllAsync();
static async Task RunAllAsync()
{
    var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .AddUserSecrets<Program>()
    .Build();

    var apiSettings = config.GetSection("ApiSettings").Get<ApiSettings>();
    if (apiSettings == null)
    {
        throw new InvalidOperationException("ApiSettings configuration section is missing or malformed.");
    }

    var token = await AuthService.GetTokenAsync(apiSettings);

    Console.WriteLine("Authenticated");

    await CreatePaymentsAsync(apiSettings, token);
    await GetPaymentsAsync(apiSettings, token);
    await GetPaymentStatusDateReportAsync(apiSettings, token);
    await CreatePaymentTemplateAsync(apiSettings, token);
    await CreatePaymentFromTemplateAsync(apiSettings, token);
    await CancelPaymentAsync(apiSettings, token);
    await RefundPaymentAsync(apiSettings, token);
}

static async Task CreatePaymentsAsync(ApiSettings settings, string token)
{
    var singleDto = PaymentDtoGenerator.Generate(settings.MerchantId);
    var single = await PaymentService.CreatePaymentAsync(singleDto, token, settings.ApiUrl);
    Console.WriteLine("Single Payment Created:");
    Console.WriteLine(JsonSerializer.Serialize(single, new JsonSerializerOptions { WriteIndented = true }));

    var bulkDtos = Enumerable.Range(0, 5)
        .Select(_ => PaymentDtoGenerator.Generate(settings.MerchantId))
        .ToList();

    var bulk = await PaymentService.BulkCreatePaymentsAsync(bulkDtos, token, settings.ApiUrl);
    Console.WriteLine("Bulk Payments Created:");
    Console.WriteLine(JsonSerializer.Serialize(bulk, new JsonSerializerOptions { WriteIndented = true }));
}

static async Task GetPaymentsAsync(ApiSettings settings, string token)
{
    var payments = await PaymentService.GetPaymentsAsync(token, settings.ApiUrl);

    Console.WriteLine("Payments Retrieved:");
    Console.WriteLine(JsonSerializer.Serialize(payments, new JsonSerializerOptions { WriteIndented = true }));
}

static async Task GetPaymentStatusDateReportAsync(ApiSettings settings, string token)
{
    var (startDate, endDate) = DateHelper.GetDateRangeForLast30Days();
    var report = await PaymentService.GetStatusDatePaymentReportAsync(startDate, endDate, token, settings.ApiUrl);

    Console.WriteLine($"Status Date Payment Report ({startDate} to {endDate}):");
    Console.WriteLine(JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
}

static async Task CreatePaymentTemplateAsync(ApiSettings settings, string token)
{
    var templateDto = PaymentTemplateDtoGenerator.Generate(settings.MerchantId);
    var result = await PaymentTemplateService.CreatePaymentTemplateAsync(templateDto, token, settings.ApiUrl);

    Console.WriteLine("Payment Template Created:");
    Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
}

static async Task CreatePaymentFromTemplateAsync(ApiSettings settings, string token)
{
    var templateDto = PaymentTemplateDtoGenerator.Generate(settings.MerchantId);
    var paymentFromTemplateDto = PaymentTemplateDtoGenerator.GenerateFromTemplate();

    var result = await PaymentTemplateService.CreatePaymentFromTemplateAsync(templateDto, paymentFromTemplateDto, token, settings.ApiUrl);

    Console.WriteLine("Payment Created from Template:");
    Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
}

static async Task CancelPaymentAsync(ApiSettings settings, string token)
{
    var dto = PaymentDtoGenerator.Generate(settings.MerchantId);
    var result = await PaymentService.CancelPaymentAsync(dto, token, settings.ApiUrl);

    Console.WriteLine("Cancel Flow:");
    Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
}

static async Task RefundPaymentAsync(ApiSettings settings, string token)
{
    var paymentId = await PaymentService.GetMostRecentPaidPaymentIdAsync(settings.MerchantId, token, settings.ApiUrl);
    var result = await PaymentService.RefundPaymentAsync(paymentId, token, settings.ApiUrl);

    Console.WriteLine("Refund Flow:");
    Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
}






