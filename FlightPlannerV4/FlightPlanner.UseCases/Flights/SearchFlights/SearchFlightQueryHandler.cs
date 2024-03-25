using AutoMapper;
using FlightPlanner.Core.Services;
using FlightPlanner.models;
using FlightPlanner.UseCases.Flights.GetFlight;
using FlightPlanner.UseCases.models;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.UseCases.Flights.SearchFlights
{
    public class SearchFlightQueryHandler : IRequestHandler<SearchFlightQuery, ServiceResults>
    {
        private readonly IFlightService _flightService;
        private readonly IValidator<SearchFlightsRequest> _validator;
        public SearchFlightQueryHandler(IFlightService flightService, IValidator<SearchFlightsRequest> validator)
        {
            _flightService = flightService;
            _validator = validator;
        }

        public async Task<ServiceResults> Handle(SearchFlightQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.Request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new ServiceResults
                {
                    ResultObject = validationResult.Errors,
                    Status = HttpStatusCode.BadRequest
                };
                
            }

            return new ServiceResults
            {
                ResultObject = _flightService.GetMatchedFlights(request.Request),
                Status = HttpStatusCode.OK
            };

        }
    }
}
