using Application.Dtos.Request.City;
using Application.Dtos.Response.City;
using AutoMapper;
using DomainCity = Domain.Entities.City;

namespace Application.Mapping
{
    public class CityMappingProfile : Profile
    {
        public CityMappingProfile()
        {
            CreateMap<DomainCity, CityResponse>()
                .ForMember(d => d.CountryName, opt => opt.MapFrom(s => s.Country != null ? s.Country.Name : null));

            CreateMap<AddCityRequest, DomainCity>();
            CreateMap<UpdateCityRequest, DomainCity>();
        }
    }
}
