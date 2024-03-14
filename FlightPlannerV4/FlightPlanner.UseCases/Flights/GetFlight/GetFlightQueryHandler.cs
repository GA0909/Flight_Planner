using AutoMapper;
using FlightPlanner.Core.Services;
using FlightPlanner.UseCases.models;
using MediatR;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.UseCases.Flights.GetFlight
{
    public class GetFlightQueryHandler : IRequestHandler<GetFlightQuery, ServiceResults>
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;

        public GetFlightQueryHandler(IFlightService flightService, IMapper mapper)
        {
            _flightService = flightService;
            _mapper = mapper;
        }
        public async Task<ServiceResults> Handle(GetFlightQuery request, CancellationToken cancellationToken)
        {
            var flight = _flightService.GetFullFlighById(request.Id);

            var response = new ServiceResults();

            if (flight == null)
            {
                response.Status = HttpStatusCode.NotFound;
                return response;
            }

            response.ResultObject = _mapper.Map<AddFlightResponse>(flight); 
            response.Status = HttpStatusCode.OK;
            return response;
        }
    }
}
