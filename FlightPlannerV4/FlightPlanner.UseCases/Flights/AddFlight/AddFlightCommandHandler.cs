using AutoMapper;
using FlightPlanner.Core.Services;
using FlightPlanner.models;
using FlightPlanner.UseCases.models;
using FlightPlanner.UseCases.Validations;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.UseCases.Flights.AddFlight
{
    public class AddFlightCommandHandler : IRequestHandler<AddFlightCommand, ServiceResults>
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddFlightRequest> _validator;
        private static readonly object _lockObject = new object();

        public AddFlightCommandHandler(IFlightService flightService, IMapper mapper, IValidator<AddFlightRequest> validator)
        {
            _flightService = flightService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ServiceResults> Handle(AddFlightCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.AddFlightRequest, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new ServiceResults 
                { 
                    ResultObject = validationResult.Errors,
                    Status = HttpStatusCode.BadRequest
                };

            }
            lock (_lockObject)
            {
                if (_flightService.FlightExists(_mapper.Map<Flight>(request.AddFlightRequest)))
                {
                    return new ServiceResults
                    {
                        ResultObject = request.AddFlightRequest,
                        Status = HttpStatusCode.Conflict
                    };

                }

                var flight = _mapper.Map<Flight>(request.AddFlightRequest);
                _flightService.Create(flight);

                return new ServiceResults
                {
                    ResultObject = _mapper.Map<AddFlightResponse>(flight),
                    Status = HttpStatusCode.Created
                };

            }
        }
    }
}
