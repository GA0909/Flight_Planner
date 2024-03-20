using AutoMapper;
using FlightPlanner.models;

namespace FlightPlanner.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<AddFlightRequest, Flight>();

            CreateMap<Airport,AirportViewModel>()
                .ForMember(viewModel => viewModel.Airport, options => 
                options.MapFrom(source => source.AirportCode));
            CreateMap<AirportViewModel, Airport>()
                .ForMember(destination => destination.AirportCode, options =>
                options.MapFrom(source => source.Airport));
            CreateMap<Flight, AddFlightResponse>();
        } 
    }
}
