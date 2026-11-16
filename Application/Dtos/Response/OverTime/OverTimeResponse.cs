namespace Application.Dtos.Response.OverTime
{
    public class OverTimeResponse
    {
        public int Id { get; set; }

        // Employee Info (as per requirements 10 & 11)
        public int EmployeeId { get; set; }
        public string? CivilId { get; set; }
        public string? FileNumber { get; set; }
        public string? Fullname { get; set; }
        public string? JobName { get; set; }
        public string? Photo { get; set; }
        public string? GeneralDepartmentName { get; set; }
        public string? DepartmentName { get; set; }
        public string? BranchName { get; set; }

        // OverTime Info
        public int OverTimeTypeId { get; set; }
        public string? OverTimeTypeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays => (EndDate - StartDate).Days;
        public string? Other { get; set; }
    }
}
