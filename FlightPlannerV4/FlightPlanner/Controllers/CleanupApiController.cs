using FlightPlanner.Core.Services;
using FlightPlanner.Extensions;
using FlightPlanner.UseCases.Cleanup;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class CleanupApiCntr : ControllerBase
    {
        private readonly IMediator _mediator;

        public CleanupApiCntr(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("clear")]
        public async Task<IActionResult> Clear()
        {
            return (await _mediator.Send(new DataCleanupCommand())).ToActionResult();
        }
    }
}
