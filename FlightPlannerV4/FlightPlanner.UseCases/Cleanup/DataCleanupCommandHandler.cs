using FlightPlanner.Core.Services;
using FlightPlanner.UseCases.models;
using MediatR;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.UseCases.Cleanup
{
    public class DataCleanupCommandHandler : IRequestHandler<DataCleanupCommand, ServiceResults>
    {
        private readonly IFlightService _flightService;
        public DataCleanupCommandHandler (IFlightService flightService)
        {
            _flightService = flightService;
        }
        public async Task<ServiceResults> Handle(DataCleanupCommand request, CancellationToken cancellationToken)
        {
            _flightService.RemoveAllFlightsAndAirports();

            return new ServiceResults();

        }
    }
}
