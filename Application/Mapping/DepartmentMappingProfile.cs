using Application.Dtos.Request.Department;
using Application.Dtos.Response.Department;
using AutoMapper;
using DomainDepartment = Domain.Entities.Department;

namespace Application.Mapping
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile()
        {
            CreateMap<DomainDepartment, DepartmentResponse>()
                .ForMember(d => d.GeneralDepartmentName, opt => opt.MapFrom(s => s.GeneralDepartment != null ? s.GeneralDepartment.Name : null));

            CreateMap<AddDepartmentRequest, DomainDepartment>();
            CreateMap<UpdateDepartmentRequest, DomainDepartment>();
        }
    }
}
