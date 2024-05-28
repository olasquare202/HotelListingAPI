using AutoMapper;
using HotelListingAPI.Model;
using HotelListingAPI.Model.DTOs;

namespace HotelListingAPI.Automapper
{
    public class Mapping : Profile
    {
        public Mapping() 
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel,  CreateHotelDto>().ReverseMap();
        }
    }
}
