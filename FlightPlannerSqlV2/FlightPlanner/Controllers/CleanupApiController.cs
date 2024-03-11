using FlightPlanner.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class CleanupApiCntr : ControllerBase
    {
        private readonly IFlightService _flightService;

        public CleanupApiCntr(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpPost]
        [Route("clear")]
        public IActionResult Clear()
        {
            _flightService.RemoveAllFlightsAndAirports();
            
            return Ok();
        }
    }
}
