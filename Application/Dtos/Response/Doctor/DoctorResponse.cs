namespace Application.Dtos.Response.Doctor
{
    public class DoctorResponse
    {
        public int Id { get; set; }

        // Employee Info (as per requirements 4, 5 & 10)
        public int EmployeeId { get; set; }
        public string? CivilId { get; set; }
        public string? FileNumber { get; set; }
        public string? Fullname { get; set; }
        public string? JobName { get; set; }
        public string? Photo { get; set; }
        public string? GeneralDepartmentName { get; set; }
        public string? DepartmentName { get; set; }
        public string? BranchName { get; set; }

        // Doctor Info
        public DateTime Data { get; set; }
        public string MedicalDiagnose { get; set; } = string.Empty;
        public string MedicalRecommendation { get; set; } = string.Empty;
        public string? Other { get; set; }
    }
}
