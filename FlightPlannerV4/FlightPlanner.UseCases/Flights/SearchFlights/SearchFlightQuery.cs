using FlightPlanner.models;
using FlightPlanner.UseCases.models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.UseCases.Flights.SearchFlights
{
    public class SearchFlightQuery : IRequest<ServiceResults>
    {
        public SearchFlightQuery(SearchFlightsRequest request)
        {
            Request = request;
        }
        public SearchFlightsRequest Request { get; }
    }
}
