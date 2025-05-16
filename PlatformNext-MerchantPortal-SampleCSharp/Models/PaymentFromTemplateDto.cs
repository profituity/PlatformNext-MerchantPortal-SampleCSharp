namespace PlatformNext_MerchantPortal_SampleCSharp.Models
{
    public class PaymentFromTemplateDto
    {
        public int Amount { get; set; }
        public string SendDate { get; set; } = null!;
        public string ExternalPaymentId { get; set; } = null!;
    }
}
