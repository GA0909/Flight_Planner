using System.Collections.Generic;
using System.Linq;
using FlightPlanner.models;

namespace FlightPlanner.storage
{
    public class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id = 1;
        private static readonly object _lockObject = new object();

        public static void AddFlight(Flight flight)
        {
            
                flight.Id = _id++;
                _flights.Add(flight);
            
        }

        public static bool FlightExists(Flight flight)
        {
            Flight[] flightsSnapshot;
            lock (_flights)
            {
                flightsSnapshot = _flights.ToArray();
            }

            return flightsSnapshot.Any(f =>
                f.Carrier == flight.Carrier &&
                f.From.AirportCode == flight.From.AirportCode &&
                f.To.AirportCode == flight.To.AirportCode &&
                f.DepartureTime == flight.DepartureTime &&
                f.ArrivalTime == flight.ArrivalTime); 
        }

        public static Flight? GetFlightById(int id)
        {
            lock (_lockObject)
            {
                if (_flights == null)
                {
                    _flights = new List<Flight>();
                }

                return _flights.FirstOrDefault(f => f.Id == id);
            }
        }

        public static void DeleteFlightById(int id)
        {
            lock (_lockObject)
            {
                var flightToRemove = GetFlightById(id);
                if (flightToRemove != null)
                {
                    _flights.Remove(flightToRemove);
                }
            }
        }

        public static void Clear()
        {
            lock (_lockObject)
            {
                _flights.Clear();
            }
        }

        public static List<Flight> GetAllFlights()
        {
            lock (_lockObject)
            {
                return _flights.ToList();
            }
        }
    }
}

