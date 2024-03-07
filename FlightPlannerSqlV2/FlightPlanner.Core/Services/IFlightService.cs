using FlightPlanner.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.Core.Services
{
    public interface IFlightService : IEntityService<Flight>
    {
        Flight? GetFullFlighById(int id);
        bool FlightExists(Flight flight);
        Airport? AirportSearch(string phrase);
        PageResult GetMatchedFlights(SearchFlightsRequest req);

    }
}
