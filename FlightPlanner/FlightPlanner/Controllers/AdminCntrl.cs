using System.ComponentModel.Design.Serialization;
using System.Globalization;
using FlightPlanner.models;
using FlightPlanner.storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminCntrl : ControllerBase
    {
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = FlightStorage.GetFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }
            
            return  Ok(flight);
        }

        [HttpPut]
        [Route("flights")]
        public IActionResult AddFlight(Flight flight)
        {
            if (flight == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(flight.Carrier) ||
                string.IsNullOrEmpty(flight.From.AirportCode) ||
                string.IsNullOrEmpty(flight.From.Country) ||
                string.IsNullOrEmpty(flight.From.City) ||
                string.IsNullOrEmpty(flight.To.AirportCode) ||
                string.IsNullOrEmpty(flight.To.Country) ||
                string.IsNullOrEmpty(flight.To.City))
            {
                return BadRequest();
            }

            if (FlightStorage.FlightExists(flight))
            {
                return Conflict();
            }

            // Normalize airport codes to lowercase for comparison
            string fromAirportCode = flight.From.AirportCode?.Trim().ToLower();
            string toAirportCode = flight.To.AirportCode?.Trim().ToLower();

            // Check if the departure and arrival airports are the same
            if (fromAirportCode == toAirportCode)
            {
                return BadRequest("Departure and arrival airports cannot be the same.");
            }

            //
            if (!DateTime.TryParseExact(flight.DepartureTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureDateTime) ||
                !DateTime.TryParseExact(flight.ArrivalTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime arrivalDateTime))
            {
                return BadRequest("Invalid date/time format. Use yyyy-MM-dd HH:mm format.");
            }

            // Check if departure time is greater than or equal to arrival time
            if (departureDateTime >= arrivalDateTime)
            {
                return BadRequest("Departure time must be earlier than arrival time.");
            }

            FlightStorage.AddFlight(flight);
            return Created("", flight);
        }


    }
}
