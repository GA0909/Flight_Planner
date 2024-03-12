using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using FlightPlanner.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.Service
{
    public class AirportService : EntityService<Airport>, IAirportService
    {
        public AirportService(IFlightPlannerDbContext context) : base(context)
        {
        }
        public List<Airport>? AirportSearch(string phrase)
        {
            string normalizedPhrase = phrase.ToLower().Replace(" ", "");
            return _context.Airports
                .Where(f => f.Country.ToLower().Contains(normalizedPhrase) ||
                                      f.City.ToLower().Contains(normalizedPhrase) ||
                                      f.AirportCode.ToLower().Contains(normalizedPhrase)).ToList();

        }


    }
}
