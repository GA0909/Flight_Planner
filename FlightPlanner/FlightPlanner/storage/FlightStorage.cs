using FlightPlanner.models;

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
            return _flights.Any(f =>
                f.Carrier == flight.Carrier &&
                f.From.AirportCode == flight.From.AirportCode &&
                f.To.AirportCode == flight.To.AirportCode &&
                f.DepartureTime == flight.DepartureTime &&
                f.ArrivalTime == flight.ArrivalTime);
        }

        public static Flight? GetFlightById(int id)
        {
          return  _flights.FirstOrDefault(flight => flight.Id == id);
        }

        public static void DeleteFlightById(int id)
        {
            _flights.Remove(GetFlightById(id));
        }
        public static void Clear()
        {
            _flights.Clear();
        }
    }
}
