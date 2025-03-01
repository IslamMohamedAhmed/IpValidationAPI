using IpValidation.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IpValidation.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly List<BlockedAttempts> blockedAttempts;

        public LogsController(List<BlockedAttempts> blockedAttempts)
        {
            this.blockedAttempts = blockedAttempts;
        }

        [HttpGet("blocked-attempts")]
        public IActionResult GetBlockedAttempts([FromQuery] int page = 1, [FromQuery] int pageSize = 3)
        {
            var paginatedList = blockedAttempts
                .OrderByDescending(a => a.Timestamp) // to order them from newest to oldest
                .Skip((page - 1) * pageSize) // apply pagination
                .Take(pageSize) // define page size
                .ToList();

            return Ok(paginatedList);
        }
    }
}
