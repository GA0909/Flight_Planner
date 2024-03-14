using FlightPlanner.models;
using FluentValidation;

namespace FlightPlanner.UseCases.Validations
{
    public class SearchFlightRequestValidator : AbstractValidator<SearchFlightsRequest>
    {
        public SearchFlightRequestValidator()
        {
            RuleFor(request => request.DepartureDate).NotEmpty();
            RuleFor(request => request.From).NotEmpty();
            RuleFor(request => request.To).NotEmpty();
            RuleFor(request => request)
                .Must(request =>
                AreDifferentAirports(request.From, request.To));

        }
        private bool AreDifferentAirports(string fromAirport, string toAirport)
        {
            if (string.IsNullOrWhiteSpace(fromAirport) || string.IsNullOrWhiteSpace(toAirport))
            {
                return true;
            }

            string fromAirportCode = fromAirport.Trim().ToLower();
            string toAirportCode = toAirport.Trim().ToLower();

            return fromAirportCode != toAirportCode;

        }
    }
}
