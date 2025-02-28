using System.Net;
using System.Text.Json;
using IpValidation.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IpValidation.Controllers
{
    [Route("api/ip")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public IpController(HttpClient httpClient,IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }
        [HttpGet("lookup")]
        public async Task<ActionResult> CountriesViaIp(string? Ip = null)
        {
            if(string.IsNullOrWhiteSpace(Ip))
            {
                Ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            }
            if(!IPAddress.TryParse(Ip,out _))
            {
                return BadRequest("error!!: Invalid IP address format.");
            }

            string url = $"https://api.ipgeolocation.io/ipgeo?apiKey={configuration["ApiKeys:ipgeolocation"]}&ip={Ip}";
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, new { error = "Failed to fetch IP details." });
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var ipInfo = JsonSerializer.Deserialize<IpResponse>(jsonResponse);

         

            return Ok(new
            {
                IPAddress = ipInfo.ip,
                ipInfo.country_name,
                ipInfo.country_code2,
                ipInfo.isp
            });
            




        }
        
    }
}
