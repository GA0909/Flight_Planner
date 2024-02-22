using System.Globalization;
using FlightPlanner.models;
using FlightPlanner.storage;
using Microsoft.AspNetCore.Mvc;


namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerClient : ControllerBase
    {
        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            string normalizedPhrase = search.ToLower().Replace(" ", "");

            List<Flight> flights = FlightStorage.GetAllFlights();

            List<Airport> matchedAirports = new List<Airport>();

            foreach (Flight flight in flights)
            {
                string fromCountry = flight.From.Country.ToLower();
                string fromCity = flight.From.City.ToLower();
                string fromAirportCode = flight.From.AirportCode.ToLower();

                string toCountry = flight.To.Country.ToLower();
                string toCity = flight.To.City.ToLower();
                string toAirportCode = flight.To.AirportCode.ToLower();

                if (fromCountry.Contains(normalizedPhrase) ||
                    fromCity.Contains(normalizedPhrase) ||
                    fromAirportCode.Contains(normalizedPhrase))
                {
                    matchedAirports.Add(flight.From);
                }

                if (toCountry.Contains(normalizedPhrase) ||
                    toCity.Contains(normalizedPhrase) ||
                    toAirportCode.Contains(normalizedPhrase))
                {
                    matchedAirports.Add(flight.To);
                }
            }

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

            List<Flight> allFlights = FlightStorage.GetAllFlights();

            List<Flight> matchingFlights = allFlights.Where(f =>
                    f.From.AirportCode == req.From &&
                    f.To.AirportCode == req.To &&
                    f.DepartureTime.Substring(0,10) == req.DepartureDate)
                .ToList();

            PageResult result = new PageResult
            {
                Page = 0, 
                TotalItems = matchingFlights.Count,
                Items = matchingFlights
            };

            return Ok(result);

        }
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlightById(int id)
        {
            var flight = FlightStorage.GetFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }



    }
}
