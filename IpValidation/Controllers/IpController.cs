using System.Net;
using System.Text.Json;
using IpValidation.Astracctions;
using IpValidation.Services;
using IpValidation.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IpValidation.Controllers
{
    [Route("api/ip")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly GeolocationService geoService;
        private readonly ILogger<IpController> logger;
        private readonly List<BlockedAttempts> blockedAttempts;

        public IpController(GeolocationService geoService, ILogger<IpController> logger, List<BlockedAttempts> blockedAttempts)
        {
            this.geoService = geoService;
            this.logger = logger;
            this.blockedAttempts = blockedAttempts;
        }
        [HttpGet("lookup")]
        public async Task<ActionResult> CountriesViaIp(string? Ip = null)
        {
            if(string.IsNullOrWhiteSpace(Ip))
            {
                Ip = geoService.GetClientIp(HttpContext);
            }
            if(!IPAddress.TryParse(Ip,out _))
            {
                return BadRequest("error!!: Invalid IP address format.");
            }

           

            
            var ipInfo = await geoService.GetCountryCodeFromIp(Ip);



            return Ok(new
            {
                IPAddress = ipInfo.ip,
                ipInfo.country_name,
                ipInfo.country_code2,
                ipInfo.isp
            });

           



        }

        [HttpGet("check-block")]
        public async Task<ActionResult> VerifyIp()
        {
            string ip = geoService.GetClientIp(HttpContext);

            if(ip == "::1")
            {
                // this part was necessary because i can't send my machine RemoteIpaddress to a wesite this is insecure 
                ip = "1.1.1.1";
            }
            else if (!IPAddress.TryParse(ip,out _))
            {
                return BadRequest("Ip is ot found");
            }
            var ipInfo = await geoService.GetCountryCodeFromIp(ip);
            if (ipInfo == null)
            {
                logger.LogError("Failed to retrieve IP information.");
                return BadRequest("Ip can't e retrieved!");  // Or handle it accordingly
            }
            string countryCode = ipInfo.country_code2;
            if (countryCode == null)
            {
                return StatusCode(500, "Code fetching execution failed!");
            }
            bool isBlocked = geoService.countries.Keys.Contains(countryCode);
            var attempt = new BlockedAttempts
            {
                IpAddress = ip,
                Timestamp = DateTime.Now,
                CountryCode = countryCode,
                BlockedStatus = isBlocked ? "Blocked" : "Allowed",
                UserAgent = Request.Headers["User-Agent"].ToString()
            };

            blockedAttempts.Add(attempt);
            logger.LogInformation($"Ip {ip} was checked - state: {countryCode} - Block: {attempt.BlockedStatus}");

            if (isBlocked)
            {
                return StatusCode(403, "Forbidden");
            }

            return Ok("Access Allowed.");

        }

    }
}
