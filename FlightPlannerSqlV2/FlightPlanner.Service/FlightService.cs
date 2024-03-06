using FlightPlanner.Core.Services;
using FlightPlanner.Core.Models;
using Microsoft.EntityFrameworkCore;
using FlightPlanner.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightPlanner.Data;

namespace FlightPlanner.Service
{
    public class FlightService : EntityService<Flight>, IFlightService
    {
        public FlightService(IFlightPlannerDbContext context): base(context) 
        {
        }
        public Flight? GetFullFlighById(int id)
        {
            return _context.Flights.Include(f => f.From).Include(f => f.To)
                .SingleOrDefault(f => f.Id == id);
        }

        public bool FlightExists(Flight flight)
        {
            return _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .Any(f => f.From.AirportCode == flight.From.AirportCode &&
                          f.To.AirportCode == flight.To.AirportCode &&
                          f.DepartureTime == flight.DepartureTime &&
                          f.ArrivalTime == flight.ArrivalTime);
        }

    }
}
