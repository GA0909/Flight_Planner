using AutoMapper;
using FlightPlanner.Core.Services;
using FlightPlanner.UseCases.models;
using MediatR;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.UseCases.Flights.DeleteFlight
{
    public class DeleteFlightQueryHandler : IRequestHandler<DeleteFlightQuery, ServiceResults>
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;

        public DeleteFlightQueryHandler(IFlightService flightService, IMapper mapper)
        {
            _flightService = flightService;
            _mapper = mapper;
        }
        public async Task<ServiceResults> Handle(DeleteFlightQuery request, CancellationToken cancellationToken)
        {
            var flightToRemove = _flightService.GetFullFlighById(request.Id);

            var response = new ServiceResults();

            if (flightToRemove != null)
            {
                _flightService.Delete(flightToRemove);
            }

            return response;
            
        }
    }
}
