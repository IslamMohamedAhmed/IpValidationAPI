using System.Collections.Concurrent;
using System.Net.Http;
using System.Text.Json;
using IpValidation.Shared;

namespace IpValidation.Services
{
    public class GeolocationService
    {
        
        private readonly ILogger<GeolocationService> logger;
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;
        public readonly ConcurrentDictionary<string, string> countries = new ConcurrentDictionary<string, string>
        {
            ["US"] = "United States",
            ["CA"] = "Canada",
            ["GB"] = "United Kingdom",
            ["FR"] = "France",
            ["DE"] = "Germany",
            ["IN"] = "India",
            ["CN"] = "China",
            ["JP"] = "Japan",
            ["BR"] = "Brazil",
            ["AU"] = "Australia",
            ["IT"] = "Italy",
            ["RU"] = "Russia",
            ["ZA"] = "South Africa",
            ["MX"] = "Mexico",
            ["KR"] = "South Korea",
            ["ES"] = "Spain"
            // reference countries to search from 
        };

        public GeolocationService(ILogger<GeolocationService> logger,IConfiguration configuration,IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
        }

        public string GetClientIp(HttpContext context)
        {
            string ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.LocalIpAddress?.ToString();
            }
            return ip;
        }

        public async Task<IpResponse> GetCountryCodeFromIp(string ip)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                string url = $"https://api.ipgeolocation.io/ipgeo?apiKey={configuration["ApiKeys:ipgeolocation"]}&ip={ip}";
                var response = await client.GetStringAsync(url);
                var json = JsonSerializer.Deserialize<IpResponse>(response);
                return json;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Code Fetch Failed!");
                return null;
            }
        }
    }
}
