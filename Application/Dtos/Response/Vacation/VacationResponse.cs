using Domain.Entities.Enum;

namespace Application.Dtos.Response.Vacation
{
    public class VacationResponse
    {
        public int Id { get; set; }

        // Employee Info
        public int EmployeeId { get; set; }
        public string? CivilId { get; set; }
        public string? FileNumber { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? JobName { get; set; }
        public string? Photo { get; set; }
        public string? GeneralDepartmentName { get; set; }
        public string? DepartmentName { get; set; }
        public string? BranchName { get; set; }

        // Vacation Info
        public int VacationTypeId { get; set; }
        public string? VacationTypeName { get; set; }
        public DateTime StartData { get; set; }
        public DateTime EndData => NumberOfDays > 0 ? StartData.AddDays(NumberOfDays - 1) : StartData;
        public int NumberOfDays { get; set; }
        public string? Other { get; set; }

        // Workflow Info
        public VacationStatus Status { get; set; }
        public string StatusName => Status.ToString();
        public string? ApprovedOrRejectedBy { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? DecisionReason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
