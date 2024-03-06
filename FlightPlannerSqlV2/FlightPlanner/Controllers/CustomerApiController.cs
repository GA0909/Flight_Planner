using AutoMapper;
using FlightPlanner.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerClient : ControllerBase
    {
        private readonly FlightPlannerDbContext _context;
        private readonly IMapper _mapper;

        public CustomerClient(FlightPlannerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            // Create a list to store AirportViewModel objects
            List<AirportViewModel> airportViewModels = new List<AirportViewModel>();

            // Iterate over matchedAirports and create AirportViewModel objects
            foreach (var airport in matchedAirports)
            {
                AirportViewModel viewModel = new AirportViewModel
                {
                    Country = airport.Country,
                    City = airport.City,
                    Airport = airport.AirportCode // Assuming AirportCode should go to Airport property in AirportViewModel
                };

                // Add the created AirportViewModel to the list
                airportViewModels.Add(viewModel);
            }

            // Return the list of AirportViewModel objects
            return Ok(airportViewModels);
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
            return Ok(_mapper.Map<AddFlightResponse>(flight));
        }

    }
}
