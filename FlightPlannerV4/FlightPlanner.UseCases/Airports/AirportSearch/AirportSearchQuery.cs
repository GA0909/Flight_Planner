using FlightPlanner.UseCases.models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.UseCases.Airports.AirportSearch
{
    public class AirportSearchQuery : IRequest<ServiceResults>
    {
        public AirportSearchQuery(string search) 
        {
            Search = search;
        }
        public string Search { get; }
    }
}
