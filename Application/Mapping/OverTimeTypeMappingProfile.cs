using Application.Dtos.Request.OverTimeType;
using Application.Dtos.Response.OverTimeType;
using AutoMapper;
using DomainOverTimeType = Domain.Entities.OverTimeType;

namespace Application.Mapping
{
    public class OverTimeTypeMappingProfile : Profile
    {
        public OverTimeTypeMappingProfile()
        {
            CreateMap<DomainOverTimeType, OverTimeTypeResponse>();
            CreateMap<AddOverTimeTypeRequest, DomainOverTimeType>();
            CreateMap<UpdateOverTimeTypeRequest, DomainOverTimeType>();
        }
    }
}
