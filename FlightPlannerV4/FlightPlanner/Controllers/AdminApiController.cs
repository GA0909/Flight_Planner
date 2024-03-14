
using FlightPlanner.Extensions;
using FlightPlanner.UseCases.Flights.AddFlight;
using FlightPlanner.UseCases.Flights.DeleteFlight;
using FlightPlanner.UseCases.Flights.GetFlight;
using FlightPlanner.UseCases.models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminCntrl : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminCntrl( IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public async Task<IActionResult> GetFlight(int id)
        {
            return (await _mediator.Send(new GetFlightQuery(id))).ToActionResult();
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            return (await _mediator.Send(new DeleteFlightQuery(id))).ToActionResult();
        }

        [HttpPut]
        [Route("flights")]
        public async Task<IActionResult> AddFlight(AddFlightRequest request)
        {
            return (await _mediator.Send(new AddFlightCommand { AddFlightRequest = request })).ToActionResult();
        }

    }

}
