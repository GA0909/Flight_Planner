using AutoMapper;
using FlightPlanner.Core.Services;
using FlightPlanner.UseCases.models;
using MediatR;
using System.Net;

namespace FlightPlanner.UseCases.Airports.SearchAirports
{
    public class SearchAirportQueryHandler : IRequestHandler<SearchAirportQuery, ServiceResults>
    {
        private readonly IMapper _mapper;
        private readonly IAirportService _airportService;

        public SearchAirportQueryHandler(IMapper mapper, IAirportService airportService)
        {
            _mapper = mapper;
            _airportService = airportService;
        }

        public async Task<ServiceResults> Handle(SearchAirportQuery request, CancellationToken cancellationToken)
        {
            var matchedAirports = _airportService.AirportSearch(request.Search);

            List<AirportViewModel> airports = new List<AirportViewModel>();

            if (matchedAirports != null)
            {
                foreach (var airport in matchedAirports)
                {
                    airports.Add(_mapper.Map<AirportViewModel>(airport));
                }
            }

            return new ServiceResults
            {
                ResultObject = airports,
                Status = HttpStatusCode.OK
            };
            
        }
    }
}
