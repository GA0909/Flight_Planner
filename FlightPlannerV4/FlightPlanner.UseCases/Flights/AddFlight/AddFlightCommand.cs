using FlightPlanner.UseCases.models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.UseCases.Flights.AddFlight
{
    public class AddFlightCommand : IRequest<ServiceResults>
    {
        public AddFlightRequest AddFlightRequest { get; set; }
    }
}
