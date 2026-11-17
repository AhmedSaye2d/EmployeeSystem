using Application.Dtos.Response.Payroll;
using AutoMapper;
using DomainPayroll = Domain.Entities.Payroll;

namespace Application.Mapping
{
    public class PayrollMappingProfile : Profile
    {
        public PayrollMappingProfile()
        {
            CreateMap<DomainPayroll, PayrollResponse>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CivilId, opt => opt.MapFrom(src => src.Employee.CivilId))
                .ForMember(dest => dest.FileNumber, opt => opt.MapFrom(src => src.Employee.FileNumber))
                .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.Employee.Fullname))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Employee.Email))
                .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.Employee.JobName))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Employee.Photo))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Employee.Branch != null ? src.Employee.Branch.Name : null))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Employee.Branch != null && src.Employee.Branch.Department != null ? src.Employee.Branch.Department.Name : null))
                .ForMember(dest => dest.GeneralDepartmentName, opt => opt.MapFrom(src => src.Employee.Branch != null && src.Employee.Branch.Department != null && src.Employee.Branch.Department.GeneralDepartment != null ? src.Employee.Branch.Department.GeneralDepartment.Name : null));
        }
    }
}
