namespace PlatformNext_MerchantPortal_SampleCSharp.Models
{
    public class RefundDto
    {
        public int Amount { get; set; }
        public string ExternalId { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string SendDate { get; set; } = null!;
        public string IdentificationNumber { get; set; } = null!;
    }
}
