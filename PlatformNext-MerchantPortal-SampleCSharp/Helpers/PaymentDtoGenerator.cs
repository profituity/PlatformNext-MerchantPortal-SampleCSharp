using PlatformNext_MerchantPortal_SampleCSharp.Models;

namespace PlatformNext_MerchantPortal_SampleCSharp.Helpers
{
    public class PaymentDtoGenerator
    {
        private static readonly Random random = new();
        private static readonly string[] firstNames = { "John", "Jane", "Alex", "Emily", "Chris", "Sara" };
        private static readonly string[] lastNames = { "Doe", "Smith", "Johnson", "Lee", "Brown", "Garcia" };
        private static readonly string[] states = { "NY", "CA", "TX", "FL", "IL" };
        private static readonly string[] cities = { "NYC", "LA", "SF", "CHI", "MIA" };

        public static PaymentDto Generate(string merchantId)
        {
            return new PaymentDto
            {
                Name = $"{Pick(firstNames)} {Pick(lastNames)}",
                RouteNumber = "123456780",
                AccountNumber = RandomNumeric(12),
                AccountType = 4,
                Amount = random.Next(100, 9100),
                Type = 1,
                EntryClass = 6,
                FrontImageFile = null,
                BackImageFile = null,
                CheckNumber = RandomNumeric(4),
                IdentificationNumber = RandomString(8).ToUpper(),
                Description = $"Payment for Invoice #{RandomNumeric(4)}",
                ExternalId = $"p{RandomString(10).ToLower()}",
                TerminalCity = Pick(cities),
                TerminalState = Pick(states),
                AchPaymentTypeCode = 2,
                Micr = RandomNumeric(25),
                SendDate = DateTime.UtcNow.ToString("o"),
                IsPrenote = false,
                SameDayAchRequested = false,
                MerchantId = merchantId,
                CustomerId = null,
                CustomFieldValues = Array.Empty<object>()
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
