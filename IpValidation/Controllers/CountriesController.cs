using IpValidation.Astracctions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IpValidation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBlockingRepository blockingRepository;

        public CountriesController(IConfiguration configuration, IBlockingRepository blockingRepository)
        {
            _configuration = configuration;
            this.blockingRepository = blockingRepository;
        }
        [HttpGet("blocked")]
        public ActionResult getAllBlockings([FromQuery] int page = 1, [FromQuery] int size = 4, [FromQuery] string searching = "")
        {
            if (page > 0 && size > 0)
            {
                var res = blockingRepository.GetAll(page, size, searching);
                return Ok(res);
            }
            return BadRequest("Page or size are invalid!");
        }

        [HttpPost("block")]
        public ActionResult AddBlocked(string code)
        {
            var res = blockingRepository.Add(code);
            switch (res)
            {
                case 1:
                    return Ok("Code was added successfully!");

                case 2:
                    return BadRequest("Code was added before!");
               
                    



            }
            return BadRequest("Code is invalid!");

        }

        [HttpDelete("block/{code}")]
        public ActionResult RemoveBlocked(string code)
        {

            var res = blockingRepository.Delete(code);
            switch (res)
            {
                case 1:
                    return Ok("Code was removed successfully!");

                case 2:
                    return NotFound("this country is not blocked!");





            }
            return BadRequest("Code is invalid!");
            
        }



    }
}
