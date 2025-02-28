namespace IpValidation.Shared
{
    public class IpResponse
    {
        public string? Status { get; set; }
        public string? ip { get; set; }
        public string? country_name { get; set; }
        public string? country_code2 { get; set; }
        public string? isp { get; set; }
        public string? Message { get; set; }
    }
}
