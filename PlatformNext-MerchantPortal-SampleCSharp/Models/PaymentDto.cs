namespace PlatformNext_MerchantPortal_SampleCSharp.Models
{
    public class PaymentDto
    {
        public string Name { get; set; } = null!;
        public string RouteNumber { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public int AccountType { get; set; }
        public int Amount { get; set; }
        public int Type { get; set; }
        public int EntryClass { get; set; }
        public string? FrontImageFile { get; set; }
        public string? BackImageFile { get; set; }
        public string CheckNumber { get; set; } = null!;
        public string IdentificationNumber { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ExternalId { get; set; } = null!;
        public string TerminalCity { get; set; } = null!;
        public string TerminalState { get; set; } = null!;
        public int AchPaymentTypeCode { get; set; }
        public string Micr { get; set; } = null!;
        public string SendDate { get; set; } = null!;
        public bool IsPrenote { get; set; }
        public bool SameDayAchRequested { get; set; }
        public string? MerchantId { get; set; }
        public string? CustomerId { get; set; }
        public object[] CustomFieldValues { get; set; } = Array.Empty<object>();
    }
}
