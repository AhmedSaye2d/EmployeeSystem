using Application.Dtos.Request.Country;
using Application.Dtos.Response.Country;
using AutoMapper;
using DomainCountry = Domain.Entities.Country;

namespace Application.Mapping
{
    public class CountryMappingProfile : Profile
    {
        public CountryMappingProfile()
        {
            CreateMap<DomainCountry, CountryResponse>();
            CreateMap<AddCountryRequest, DomainCountry>();
            CreateMap<UpdateCountryRequest, DomainCountry>();
        }
    }
}
