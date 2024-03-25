using FlightPlanner.UseCases.models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.UseCases.Flights.GetFlight
{
    public class GetFlightQuery : IRequest<ServiceResults>
    {
        public GetFlightQuery(int id) 
        {
            Id = id;
        }
        public int Id { get; }
    }
}
 