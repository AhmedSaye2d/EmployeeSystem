using Application.Dtos.Response.Doctor;
using Application.Dtos.Response.Employee;
using Application.Dtos.Response.OverTime;
using Application.Dtos.Response.Sanction;
using Application.Dtos.Response.Vacation;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            // ── Employee → EmployeeResponse (قائمة خفيفة بدون Collections) ──
            CreateMap<Employee, EmployeeResponse>()
                .ForMember(d => d.BranchName,            opt => opt.MapFrom(s => s.Branch != null ? s.Branch.Name : null))
                .ForMember(d => d.DepartmentName,        opt => opt.MapFrom(s => s.Branch != null && s.Branch.Department != null ? s.Branch.Department.Name : null))
                .ForMember(d => d.GeneralDepartmentName, opt => opt.MapFrom(s => s.Branch != null && s.Branch.Department != null && s.Branch.Department.GeneralDepartment != null ? s.Branch.Department.GeneralDepartment.Name : null));

            // ── Employee → EmployeeDetailsResponse (تفاصيل كاملة + Collections) ──
            CreateMap<Employee, EmployeeDetailsResponse>()
                .ForMember(d => d.BranchName,            opt => opt.MapFrom(s => s.Branch != null ? s.Branch.Name : null))
                .ForMember(d => d.DepartmentId,          opt => opt.MapFrom(s => s.Branch != null && s.Branch.Department != null ? (int?)s.Branch.Department.Id : null))
                .ForMember(d => d.DepartmentName,        opt => opt.MapFrom(s => s.Branch != null && s.Branch.Department != null ? s.Branch.Department.Name : null))
                .ForMember(d => d.GeneralDepartmentId,   opt => opt.MapFrom(s => s.Branch != null && s.Branch.Department != null && s.Branch.Department.GeneralDepartment != null ? (int?)s.Branch.Department.GeneralDepartment.Id : null))
                .ForMember(d => d.GeneralDepartmentName, opt => opt.MapFrom(s => s.Branch != null && s.Branch.Department != null && s.Branch.Department.GeneralDepartment != null ? s.Branch.Department.GeneralDepartment.Name : null))
                .ForMember(d => d.TownName,              opt => opt.MapFrom(s => s.Town != null ? s.Town.Name : null))
                .ForMember(d => d.CityId,                opt => opt.MapFrom(s => s.Town != null && s.Town.City != null ? (int?)s.Town.City.Id : null))
                .ForMember(d => d.CityName,              opt => opt.MapFrom(s => s.Town != null && s.Town.City != null ? s.Town.City.Name : null))
                .ForMember(d => d.CountryId,             opt => opt.MapFrom(s => s.Town != null && s.Town.City != null && s.Town.City.Country != null ? (int?)s.Town.City.Country.Id : null))
                .ForMember(d => d.CountryName,           opt => opt.MapFrom(s => s.Town != null && s.Town.City != null && s.Town.City.Country != null ? s.Town.City.Country.Name : null))
                .ForMember(d => d.Vacations,             opt => opt.MapFrom(s => s.Vacations))
                .ForMember(d => d.OverTimes,             opt => opt.MapFrom(s => s.OverTimes))
                .ForMember(d => d.Doctors,               opt => opt.MapFrom(s => s.Doctors))
                .ForMember(d => d.Sanctions,             opt => opt.MapFrom(s => s.Sanctions));

            // ── Vacation → VacationResponse ────────────────────────────────────
            CreateMap<Vacation, VacationResponse>()
                .ForMember(d => d.VacationTypeName, opt => opt.MapFrom(s => s.VacationType != null ? s.VacationType.Name : null));

            CreateMap<Application.Dtos.Request.Vacation.AddVacationRequest, Vacation>();
            CreateMap<Application.Dtos.Request.Vacation.UpdateVacationRequest, Vacation>();

            // ── OverTime → OverTimeResponse ────────────────────────────────────
            CreateMap<OverTime, OverTimeResponse>()
                .ForMember(d => d.OverTimeTypeName, opt => opt.MapFrom(s => s.OverTimeType != null ? s.OverTimeType.Name : null));

            CreateMap<Application.Dtos.Request.OverTime.AddOverTimeRequest, OverTime>();
            CreateMap<Application.Dtos.Request.OverTime.UpdateOverTimeRequest, OverTime>();

            // ── Doctor → DoctorResponse ────────────────────────────────────────
            CreateMap<Doctor, DoctorResponse>();

            CreateMap<Application.Dtos.Request.Doctor.AddDoctorRequest, Doctor>();
            CreateMap<Application.Dtos.Request.Doctor.UpdateDoctorRequest, Doctor>();

            // ── Sanction → SanctionResponse ────────────────────────────────────
            // Entity اسمه Sanction ، DB table اسمه Snactions ، لكن الـ DTO اسمه SanctionResponse
            CreateMap<Sanction, SanctionResponse>()
                .ForMember(d => d.SanctionTypeName, opt => opt.MapFrom(s => s.SanctionType != null ? s.SanctionType.Name : null));

            CreateMap<Application.Dtos.Request.Sanction.AddSanctionRequest, Sanction>();
            CreateMap<Application.Dtos.Request.Sanction.UpdateSanctionRequest, Sanction>();
        }
    }
}
