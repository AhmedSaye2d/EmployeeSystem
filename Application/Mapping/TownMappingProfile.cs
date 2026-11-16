using Application.Dtos.Request.Town;
using Application.Dtos.Response.Town;
using AutoMapper;
using DomainTown = Domain.Entities.Town;

namespace Application.Mapping
{
    public class TownMappingProfile : Profile
    {
        public TownMappingProfile()
        {
            CreateMap<DomainTown, TownResponse>()
                .ForMember(d => d.CityName, opt => opt.MapFrom(s => s.City != null ? s.City.Name : null))
                .ForMember(d => d.CountryId, opt => opt.MapFrom(s => s.City != null ? s.City.CountryId : 0))
                .ForMember(d => d.CountryName, opt => opt.MapFrom(s => s.City != null && s.City.Country != null ? s.City.Country.Name : null));

            CreateMap<AddTownRequest, DomainTown>();
            CreateMap<UpdateTownRequest, DomainTown>();
        }
    }
}
