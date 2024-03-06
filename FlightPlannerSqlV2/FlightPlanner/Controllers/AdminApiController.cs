using AutoMapper;
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
        private readonly IMapper _mapper;
        private static readonly object _lockObject = new object();

        public AdminCntrl(IFlightService flightService, IMapper mapper)
        {
            _flightService = flightService;
            _mapper = mapper;
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

            return Ok(_mapper.Map<AddFlightResponse>(flight));
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            var flightToRemove = _flightService.GetFullFlighById(id);
            if (flightToRemove != null)
            {
                _flightService.Delete(flightToRemove);
            }

            


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

                if (_flightService.FlightExists(_mapper.Map<Flight>(flight)))
                {
                    return Conflict();

                }
                var fflight = _mapper.Map<Flight>(flight);
                _flightService.Create(fflight);

                return Created("",_mapper.Map<AddFlightResponse>(fflight));

            }

        }

        
    }

}
