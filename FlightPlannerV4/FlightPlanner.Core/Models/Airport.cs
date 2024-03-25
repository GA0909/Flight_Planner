using FlightPlanner.Core.Models;
using System.Text.Json.Serialization;

namespace FlightPlanner.models
{
    public class Airport : Entity
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string AirportCode { get; set; }
    }
}
             