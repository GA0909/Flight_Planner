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

        public List<Airport>? AirportSearch(string phrase)
        {
            string normalizedPhrase = phrase.ToLower().Replace(" ", "");
            return _context.Airports
                .Where(f => f.Country.ToLower().Contains(normalizedPhrase) ||
                                      f.City.ToLower().Contains(normalizedPhrase) ||
                                      f.AirportCode.ToLower().Contains(normalizedPhrase)).ToList();
        
        }

        public PageResult GetMatchedFlights(SearchFlightsRequest req)
        {
            // Perform the LINQ query to retrieve matched flights
            var matchedFlights = _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .Where(f =>
                    f.From.AirportCode == req.From &&
                    f.To.AirportCode == req.To &&
                    f.DepartureTime.Substring(0, 10) == req.DepartureDate)
                .ToList();

            PageResult result = new PageResult
            {
                Page = 0,
                TotalItems = matchedFlights.Count,
                Items = matchedFlights
            };

            return result;

        }

    }
}
