using Application.Dtos.Request.GeneralDepartment;
using Application.Dtos.Response.GeneralDepartment;
using AutoMapper;
using DomainGeneralDepartment = Domain.Entities.GeneralDepartment;

namespace Application.Mapping
{
    public class GeneralDepartmentMappingProfile : Profile
    {
        public GeneralDepartmentMappingProfile()
        {
            CreateMap<DomainGeneralDepartment, GeneralDepartmentResponse>();
            CreateMap<AddGeneralDepartmentRequest, DomainGeneralDepartment>();
            CreateMap<UpdateGeneralDepartmentRequest, DomainGeneralDepartment>();
        }
    }
}
