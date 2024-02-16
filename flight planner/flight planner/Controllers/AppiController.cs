using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace flight_planner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AppiController : ControllerBase
    {
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            return Ok();
        }
    }
}
