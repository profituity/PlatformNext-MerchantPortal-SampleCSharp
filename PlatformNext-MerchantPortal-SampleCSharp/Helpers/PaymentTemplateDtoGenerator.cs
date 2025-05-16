using PlatformNext_MerchantPortal_SampleCSharp.Models;

namespace PlatformNext_MerchantPortal_SampleCSharp.Helpers
{
    public class PaymentTemplateDtoGenerator
    {
        private static readonly Random random = new();
        private static readonly string[] templateNames = { "Monthly Plan", "Weekly Debit", "Installment Payment" };

        public static PaymentTemplateDto Generate(string merchantId)
        {
            var name = $"{Pick(templateNames)} Template";

            return new PaymentTemplateDto
            {
                ExternalId = $"PT-{RandomNumeric(5)}",
                Name = name,
                RouteNumber = "123456780",
                AccountNumber = RandomNumeric(9),
                AccountType = 4,
                Amount = random.Next(100, 600),
                Type = 1,
                EntryClass = 6,
                AchPaymentTypeCode = 2,
                Description = $"Template for {name}",
                IdentificationNumber = RandomString(12).ToLower(),
                RecurrencePattern = 1,
                NonBankingDayRule = null,
                Frequency = null,
                DayOfMonth = null,
                Duration = null,
                DurationAmount = null,
                SendPreNote = false,
                SameDayAchRequested = false,
                StartDate = null,
                IsActive = true,
                MerchantId = merchantId,
                CustomerId = null,
                CustomFieldValues = Array.Empty<object>()
            };
        }

        public static PaymentFromTemplateDto GenerateFromTemplate()
        {
            return new PaymentFromTemplateDto
            {
                Amount = random.Next(100, 600),
                SendDate = DateTime.UtcNow.ToString("o"),
                ExternalPaymentId = $"ext-{RandomString(8)}"
            };
        }

        private static string Pick(string[] array) => array[random.Next(array.Length)];

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string RandomNumeric(int length)
        {
            const string digits = "0123456789";
            return new string(Enumerable.Repeat(digits, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
