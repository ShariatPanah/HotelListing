using AutoMapper;
using HotelListing.Core;

namespace HotelListing.Dtos.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CreateCountryDto>().ReverseMap();

            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();

            CreateMap<AppUser, AppUserDto>().ReverseMap();
        }
    }
}
