using FlightPlanner.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerClient : ControllerBase
    {
        private readonly FlightPlannerDbContex _context;

        public CustomerClient(FlightPlannerDbContex context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            string normalizedPhrase = search.ToLower().Replace(" ", "");

            var matchedAirports = _context.Airports
                .Where(f => f.Country.ToLower().Contains(normalizedPhrase) ||
                            f.City.ToLower().Contains(normalizedPhrase) ||
                            f.AirportCode.ToLower().Contains(normalizedPhrase)).ToList();
                
            return Ok(matchedAirports);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightsRequest req)
        {
            
            if (req.DepartureDate == null || req.From == null || req.To == null || req.From == req.To)
            {
                return BadRequest();
            }

            var matchedFlights = _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .Where(f =>
                    f.From.AirportCode == req.From &&
                    f.To.AirportCode == req.To &&
                    f.DepartureTime.Substring(0,10) == req.DepartureDate)
                .ToList();

            PageResult result = new PageResult
            {
                Page = 0, 
                TotalItems = matchedFlights.Count,
                Items = matchedFlights
            };

            return Ok(result);

        }
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlightById(int id)
        {
            var flight = _context.Flights.Include(flight => flight.To)
                .Include(flight => flight.From)
                .SingleOrDefault(flight => flight.Id == id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }

    }
}
