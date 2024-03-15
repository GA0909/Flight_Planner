using FlightPlanner.UseCases.models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.UseCases.Airports.SearchAirports
{
    public class SearchAirportQuery : IRequest<ServiceResults>
    {
        public SearchAirportQuery(string search) 
        {
            Search = search;
        }
        public string Search { get; }
    }
}
