namespace PlatformNext_MerchantPortal_SampleCSharp.Models
{
    public class PaymentTemplateDto
    {
        public string ExternalId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string RouteNumber { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public int AccountType { get; set; }
        public int Amount { get; set; }
        public int Type { get; set; }
        public int EntryClass { get; set; }
        public int AchPaymentTypeCode { get; set; }
        public string Description { get; set; } = null!;
        public string IdentificationNumber { get; set; } = null!;
        public int? RecurrencePattern { get; set; }
        public int? NonBankingDayRule { get; set; }
        public int? Frequency { get; set; }
        public int? DayOfMonth { get; set; }
        public int? Duration { get; set; }
        public int? DurationAmount { get; set; }
        public bool SendPreNote { get; set; }
        public bool SameDayAchRequested { get; set; }
        public string? StartDate { get; set; }
        public bool IsActive { get; set; }
        public string? MerchantId { get; set; }
        public string? CustomerId { get; set; }
        public object[] CustomFieldValues { get; set; } = Array.Empty<object>();
    }
}
