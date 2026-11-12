namespace Domain.Entities
{
    public class OtherBaseEntity
    {
        public int Id { get; set; }

        // FK → Employee
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public string? Other { get; set; }

        // Employee Info navigation properties for clean DTO auto-mapping
        public string? CivilId => Employee?.CivilId;
        public string? FileNumber => Employee?.FileNumber;
        public string? Fullname => Employee?.Fullname;
        public string? Email => Employee?.Email;
        public string? JobName => Employee?.JobName;
        public string? Photo => Employee?.Photo;
        public string? BranchName => Employee?.Branch?.Name;
        public string? DepartmentName => Employee?.Branch?.Department?.Name;
        public string? GeneralDepartmentName => Employee?.Branch?.Department?.GeneralDepartment?.Name;
    }
}
