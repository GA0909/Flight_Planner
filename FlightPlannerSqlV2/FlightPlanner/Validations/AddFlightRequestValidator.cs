using FlightPlanner.models;
using FluentValidation;
using System.Globalization;

namespace FlightPlanner.Validations
{
    public class AddFlightRequestValidator : AbstractValidator<AddFlightRequest>
    {
        public AddFlightRequestValidator()
        {
            RuleFor(request => request.Carrier).NotEmpty();
            RuleFor(request => request.ArrivalTime).NotEmpty();
            RuleFor(request => request.DepartureTime).NotEmpty();
            RuleFor(request => request.To).SetValidator(new AirportViewModelValidator());
            RuleFor(request => request.From).SetValidator(new AirportViewModelValidator());

            // Custom validation for airport codes
            RuleFor(request => request)
                .Must(request => 
                AreDifferentAirports(request.From.Airport, request.To.Airport));

            // Custom validation for date and time
            RuleFor(request => request)
                .Must(request => 
                AreValidDateTime(request.DepartureTime, request.ArrivalTime));

            RuleFor(request => request)
                .Must(request => 
                IsDepartureBeforeArrival(request.DepartureTime, request.ArrivalTime));
        }

        private bool AreDifferentAirports(string fromAirport, string toAirport)
        {
            string fromAirportCode = fromAirport.Trim().ToLower();
            string toAirportCode = toAirport.Trim().ToLower();
            return fromAirportCode != toAirportCode;
        }

        private bool AreValidDateTime(string departureTime, string arrivalTime)
        {
            return DateTime.TryParseExact(departureTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _) &&
                   DateTime.TryParseExact(arrivalTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        private bool IsDepartureBeforeArrival(string departureTime, string arrivalTime)
        {
            if (!DateTime.TryParseExact(departureTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureDateTime) ||
                !DateTime.TryParseExact(arrivalTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime arrivalDateTime))
            {
                return false;
            }

            return departureDateTime < arrivalDateTime;
        }
    }
}
