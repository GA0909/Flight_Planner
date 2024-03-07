using AutoMapper;
using FlightPlanner.Core.Services;
using FlightPlanner.models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerClient : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;
        private readonly IValidator<SearchFlightsRequest> _validator;

        public CustomerClient(IFlightService flightService, IMapper mapper,IValidator<SearchFlightsRequest> validator)
        {
            _flightService = flightService;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            var matchedAirport = _flightService.AirportSearch(search);

            List<AirportViewModel> airports = new List<AirportViewModel>();

            if(matchedAirport != null) 
            {
                airports.Add(_mapper.Map<AirportViewModel>(matchedAirport));
            }
           
            return Ok(airports);

        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightsRequest req)
        {
            var validationResult = _validator.Validate(req);

            if (!validationResult.IsValid)
            {
                return BadRequest();
            }

            var result = _flightService.GetMatchedFlights(req);

            return Ok(result);

        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlightById(int id)
        {
            var flight = _flightService.GetFullFlighById(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AddFlightResponse>(flight));

        }

    }
}
