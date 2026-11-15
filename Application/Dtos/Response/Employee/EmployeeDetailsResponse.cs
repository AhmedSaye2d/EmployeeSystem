using Application.Dtos.Response.Doctor;
using Application.Dtos.Response.OverTime;
using Application.Dtos.Response.Sanction;
using Application.Dtos.Response.Vacation;

namespace Application.Dtos.Response.Employee
{
    /// <summary>
    /// Response شامل لتفاصيل موظف واحد (GetById)
    /// </summary>
    public class EmployeeDetailsResponse
    {
        // ── Employee Information ───────────────────────────────────────
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CivilId { get; set; } = string.Empty;
        public string FileNumber { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string JobName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string TelephoneNumber { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public string? Other { get; set; }

        // ── Organizational Information ─────────────────────────────────
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? GeneralDepartmentId { get; set; }
        public string? GeneralDepartmentName { get; set; }

        // ── Location Information ───────────────────────────────────────
        public int TownId { get; set; }
        public string? TownName { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }

        // ── Related Collections ────────────────────────────────────────
        public List<VacationResponse> Vacations { get; set; } = new();
        public List<OverTimeResponse> OverTimes { get; set; } = new();
        public List<DoctorResponse> Doctors { get; set; } = new();
        public List<SanctionResponse> Sanctions { get; set; } = new();
    }
}
