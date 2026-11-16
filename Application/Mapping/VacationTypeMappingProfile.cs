using Application.Dtos.Request.VacationType;
using Application.Dtos.Response.VacationType;
using AutoMapper;
using DomainVacationType = Domain.Entities.VacationType;

namespace Application.Mapping
{
    public class VacationTypeMappingProfile : Profile
    {
        public VacationTypeMappingProfile()
        {
            CreateMap<DomainVacationType, VacationTypeResponse>();
            CreateMap<AddVacationTypeRequest, DomainVacationType>();
            CreateMap<UpdateVacationTypeRequest, DomainVacationType>();
        }
    }
}
