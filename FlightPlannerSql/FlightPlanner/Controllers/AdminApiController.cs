using System.ComponentModel.Design.Serialization;
using System.Globalization;
using FlightPlanner.models;
using FlightPlanner.storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminCntrl : ControllerBase
    {
        private readonly FlightPlannerDbContex _context;

        public AdminCntrl(FlightPlannerDbContex context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
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

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            FlightStorage.DeleteFlightById(id);

            return Ok();
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

            if (SqlFlightExists(flight))
            {
                return Conflict();
            }

            string fromAirportCode = flight.From.AirportCode.Trim().ToLower();
            string toAirportCode = flight.To.AirportCode.Trim().ToLower();


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

            _context.Flights.Add(flight);
            _context.SaveChanges();
            return Created("", flight);
        }
        public bool SqlFlightExists(Flight flight)
        {
            return _context.Flights.Include(f => f.To).Include(f => f.From).Any(f =>
                f.Carrier == flight.Carrier &&
                f.From.AirportCode == flight.From.AirportCode &&
                f.To.AirportCode == flight.To.AirportCode &&
                f.DepartureTime == flight.DepartureTime &&
                f.ArrivalTime == flight.ArrivalTime);
            
        }



    }

    



}
