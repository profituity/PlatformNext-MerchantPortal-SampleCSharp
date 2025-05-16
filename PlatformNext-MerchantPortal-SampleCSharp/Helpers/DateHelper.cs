namespace PlatformNext_MerchantPortal_SampleCSharp.Helpers
{
    public class DateHelper
    {
        public static (string StartDate, string EndDate) GetDateRangeForLast30Days()
        {
            var now = DateTime.UtcNow;
            var start = now.AddDays(-30);
            return (start.ToString("o"), now.ToString("o"));
        }
    }
}
