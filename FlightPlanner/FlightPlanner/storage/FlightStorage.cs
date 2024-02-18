﻿using FlightPlanner.models;

namespace FlightPlanner.storage
{
    public class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id = 1;

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
            if (_flights == null)
            {

                _flights = new List<Flight>();
            }

            return _flights.FirstOrDefault(f => f.Id == id);
        }

        public static void DeleteFlightById(int id)
        {
            var flightToRemove = GetFlightById(id);
            if (flightToRemove != null)
            {
                _flights.Remove(flightToRemove);
            }
        }
        public static void Clear()
        {
            _flights.Clear();
        }
        public static List<Flight> GetAllFlights()
        {
            return _flights.ToList();
        }
    }
}
