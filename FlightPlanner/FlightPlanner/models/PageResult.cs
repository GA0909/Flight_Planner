namespace FlightPlanner.models
{
    public class PageResult
    {
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public List<Flight> Items { get; set; }

        // Constructor
        public PageResult()
        {
            // Initialize Items to an empty list
            Items = new List<Flight>();
        }
    }
}
