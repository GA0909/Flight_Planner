using AutoMapper;
using FlightPlanner.Core.Services;
using FlightPlanner.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerClient : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;

        public CustomerClient(IFlightService flightService, IMapper mapper)
        {
            _flightService = flightService;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            var matchedAirport = _flightService.AirportSearch(search);

            // Create a list to store AirportViewModel objects
            List<AirportViewModel> airportViewModels = new List<AirportViewModel>();

            if (matchedAirport != null)
            {
                // Create AirportViewModel object for the matched airport
                AirportViewModel viewModel = new AirportViewModel
                {
                    Country = matchedAirport.Country,
                    City = matchedAirport.City,
                    Airport = matchedAirport.AirportCode // Assuming AirportCode should go to Airport property in AirportViewModel
                };

                // Add the created AirportViewModel to the list
                airportViewModels.Add(viewModel);
            }

            // Return the list of AirportViewModel objects
            return Ok(airportViewModels);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightsRequest req)
        {
            
            if (req.DepartureDate == null || req.From == null || req.To == null || req.From == req.To)
            {
                return BadRequest();
            }

            var matchedFlights = _flightService.GetMatchedFlights(req);

            // Create a PageResult object
            PageResult result = new PageResult
            {
                Page = 0,
                TotalItems = matchedFlights.Count,
                Items = matchedFlights
            };

            // Return the PageResult
            return Ok(result);

        }
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlightById(int id)
        {
            var flight = _flightService.GetFullFlighById(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<AddFlightResponse>(flight));
        }

    }
}
