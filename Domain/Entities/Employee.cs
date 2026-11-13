using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Employee : BaseEntity
    {
        [Required]
        public string? CivilId { get; set; }
        [Required]
        public string? FileNumber { get; set; }
        [Required]
        public string? Fullname { get; set; }
        [Required]
        public string? JobName { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? TelephoneNumber { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Photo { get; set; }
        public string? Other { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get; set; } = 0m;

        // Relationships
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;

        public int TownId { get; set; }
        public Town Town { get; set; } = null!;

        // Cascading Navigation Properties for clean DTO auto-mapping
        public string? BranchName => Branch?.Name;
        public int? DepartmentId => Branch?.DepartmentId ?? Branch?.Department?.Id;
        public string? DepartmentName => Branch?.Department?.Name;
        public int? GeneralDepartmentId => Branch?.Department?.GeneralDepartmentId ?? Branch?.Department?.GeneralDepartment?.Id;
        public string? GeneralDepartmentName => Branch?.Department?.GeneralDepartment?.Name;

        public string? TownName => Town?.Name;
        public int? CityId => Town?.CityId ?? Town?.City?.Id;
        public string? CityName => Town?.City?.Name;
        public int? CountryId => Town?.City?.CountryId ?? Town?.City?.Country?.Id;
        public string? CountryName => Town?.City?.Country?.Name;

        // One-to-Many collections
        public List<Vacation> Vacations { get; set; } = new();
        public List<OverTime> OverTimes { get; set; } = new();
        public List<Doctor> Doctors { get; set; } = new();
        public List<Sanction> Sanctions { get; set; } = new();
        public List<Payroll> Payrolls { get; set; } = new();
    }
}
