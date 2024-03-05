using FlightPlanner.Core.Services;
using FlightPlanner.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminCntrl : ControllerBase
    {
        private readonly IFlightService _flightService;
        private static readonly object _lockObject = new object();

        public AdminCntrl(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = _flightService.GetFullFlighById(id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            var flightToRemove = _context.Flights.Include(flight => flight.To)
                .Include(flight => flight.From)
                .SingleOrDefault(flight => flight.Id == id);
            if (flightToRemove != null)
            {
                _context.Flights.Remove(flightToRemove);
                _context.Airports.Remove(flightToRemove.From);
                _context.Airports.Remove(flightToRemove.To);
            }

            _context.SaveChanges();


            return Ok();
        }
        [HttpPut]
        [Route("flights")]
        public IActionResult AddFlight(AddFlightRequest flight)
        {
            lock (_lockObject)
            {
                if (flight == null ||
                    string.IsNullOrEmpty(flight.Carrier) ||
                    string.IsNullOrEmpty(flight.From.Airport) ||
                    string.IsNullOrEmpty(flight.From.Country) ||
                    string.IsNullOrEmpty(flight.From.City) ||
                    string.IsNullOrEmpty(flight.To.Airport) ||
                    string.IsNullOrEmpty(flight.To.Country) ||
                    string.IsNullOrEmpty(flight.To.City))
                {
                    return BadRequest();
                }

                string fromAirportCode = flight.From.Airport.Trim().ToLower();
                string toAirportCode = flight.To.Airport.Trim().ToLower();

                if (fromAirportCode == toAirportCode)
                {
                    return BadRequest();
                }

                if (!DateTime.TryParseExact(flight.DepartureTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureDateTime) ||
                    !DateTime.TryParseExact(flight.ArrivalTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime arrivalDateTime))
                {
                    return BadRequest();
                }

                if (departureDateTime >= arrivalDateTime)
                {
                    return BadRequest();
                }

                if (SqlFlightExists(flight))
                {
                    return Conflict();

                }

                _flightService.Create(flight);

                return Created("", flight);

            }

        }

        public bool SqlFlightExists(Flight flight)
        {
            return _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .Any(f => f.From.AirportCode == flight.From.AirportCode &&
                          f.To.AirportCode == flight.To.AirportCode &&
                          f.DepartureTime == flight.DepartureTime &&
                          f.ArrivalTime == flight.ArrivalTime);

        }
    }

}
