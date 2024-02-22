using FlightPlanner.models;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner
{
    public class FlightPlannerDbContex: DbContext
    {
        public FlightPlannerDbContex(DbContextOptions<FlightPlannerDbContex> options) : base(options)
        {


        }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airport> Airports { get; set; }
    }
}
