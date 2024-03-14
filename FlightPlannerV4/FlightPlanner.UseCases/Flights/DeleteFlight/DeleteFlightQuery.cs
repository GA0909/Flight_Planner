using FlightPlanner.UseCases.models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.UseCases.Flights.DeleteFlight

{
    public class DeleteFlightQuery : IRequest<ServiceResults>
    {
        public DeleteFlightQuery(int id) 
        {
            Id = id;
        }
        public int Id { get; }
    }
}
 