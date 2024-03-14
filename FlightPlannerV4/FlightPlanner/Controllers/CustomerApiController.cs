using AutoMapper;
using FlightPlanner.Core.Services;
using FlightPlanner.Extensions;
using FlightPlanner.models;
using FlightPlanner.UseCases.Flights.GetFlight;
using FlightPlanner.UseCases.models;
using FluentValidation;
using MediatR;
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
        private readonly IAirportService _airportService;
        private readonly IMediator _mediator;

        public CustomerClient(IMediator mediator ,IFlightService flightService, IMapper mapper,IValidator<SearchFlightsRequest> validator, IAirportService airportService)
        {
            _mediator = mediator;
            _flightService = flightService;
            _mapper = mapper;
            _validator = validator;
            _airportService = airportService;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            var matchedAirports = _airportService.AirportSearch(search);

            List<AirportViewModel> airports = new List<AirportViewModel>();

            if(matchedAirports != null) 
            {
                foreach(var airport in matchedAirports)
                {
                    airports.Add(_mapper.Map<AirportViewModel>(airport));
                }
               
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
        public async Task<IActionResult> GetFlight(int id)
        {
            return (await _mediator.Send(new GetFlightQuery(id))).ToActionResult();
        }

    }
}
