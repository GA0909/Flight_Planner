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
            // Normalize the search phrase to lowercase and remove spaces
            string normalizedPhrase = search.ToLower().Replace(" ", "");

            // Retrieve all flights from storage
            List<Flight> flights = FlightStorage.GetAllFlights();

            // List to hold matched airports
            List<Airport> matchedAirports = new List<Airport>();

            // Iterate through each flight
            foreach (Flight flight in flights)
            {
                // Normalize airport properties to lowercase and remove spaces
                string fromCountry = flight.From.Country.ToLower();
                string fromCity = flight.From.City.ToLower();
                string fromAirportCode = flight.From.AirportCode.ToLower();

                string toCountry = flight.To.Country.ToLower();
                string toCity = flight.To.City.ToLower();
                string toAirportCode = flight.To.AirportCode.ToLower();

                // Check if the search phrase matches any part of the departure airport
                if (fromCountry.Contains(normalizedPhrase) ||
                    fromCity.Contains(normalizedPhrase) ||
                    fromAirportCode.Contains(normalizedPhrase))
                {
                    matchedAirports.Add(flight.From);
                }

                // Check if the search phrase matches any part of the arrival airport
                if (toCountry.Contains(normalizedPhrase) ||
                    toCity.Contains(normalizedPhrase) ||
                    toAirportCode.Contains(normalizedPhrase))
                {
                    matchedAirports.Add(flight.To);
                }
            }

            // Return matched airports
            return Ok(matchedAirports);
        }



    }
}
