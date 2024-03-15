using AutoMapper;
using FlightPlanner.Core.Services;
using FlightPlanner.Extensions;
using FlightPlanner.models;
using FlightPlanner.UseCases.Airports.SearchAirports;
using FlightPlanner.UseCases.Flights.GetFlight;
using FlightPlanner.UseCases.Flights.SearchFlights;
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
        private readonly IMediator _mediator;

        public CustomerClient(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("airports")]
        public async Task<IActionResult> SearchAirports(string search)
        {
            return (await _mediator.Send(new SearchAirportQuery(search))).ToActionResult();
        }

        [HttpPost]
        [Route("flights/search")]
        public async Task<IActionResult> SearchFlights(SearchFlightsRequest req)
        {
            return (await _mediator.Send(new SearchFlightQuery(req))).ToActionResult();
        }

        [HttpGet]
        [Route("flights/{id}")]
        public async Task<IActionResult> GetFlight(int id)
        {
            return (await _mediator.Send(new GetFlightQuery(id))).ToActionResult();
        }

    }
}
