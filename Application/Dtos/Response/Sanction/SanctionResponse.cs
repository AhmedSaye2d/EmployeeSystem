namespace Application.Dtos.Response.Sanction
{
    public class SanctionResponse
    {
        public int Id { get; set; }

        // Employee Info (as per requirement 16)
        public int EmployeeId { get; set; }
        public string? CivilId { get; set; }
        public string? FileNumber { get; set; }
        public string? Fullname { get; set; }
        public string? JobName { get; set; }
        public string? Photo { get; set; }
        public string? GeneralDepartmentName { get; set; }
        public string? DepartmentName { get; set; }
        public string? BranchName { get; set; }

        // Sanction Info
        public int SanctionTypeId { get; set; }
        public string? SanctionTypeName { get; set; }
        public DateTime Data { get; set; }
        public string Punishment { get; set; } = string.Empty;
        public DateTime PunishmentDate { get; set; }
        public string? Other { get; set; }
    }
}
