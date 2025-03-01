namespace IpValidation.Shared
{
    public class BlockedAttempts
    {
        public string IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
        public string CountryCode { get; set; }
        public string BlockedStatus { get; set; }
        public string UserAgent { get; set; }
    }
}
