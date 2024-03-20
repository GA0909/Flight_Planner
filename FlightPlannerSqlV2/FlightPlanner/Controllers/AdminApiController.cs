using AutoMapper;
using FlightPlanner.Core.Services;
using FlightPlanner.models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminCntrl : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddFlightRequest> _validator;
        private static readonly object _lockObject = new object();

        public AdminCntrl(IFlightService flightService, IMapper mapper,IValidator<AddFlightRequest> validator)
        {
            _flightService = flightService;
            _mapper = mapper;
            _validator = validator;
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
                var validationResult = _validator.Validate(flight);

                if (!validationResult.IsValid)
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
