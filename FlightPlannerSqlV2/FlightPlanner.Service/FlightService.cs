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
    }
}
