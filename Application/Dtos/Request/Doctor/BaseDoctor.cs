using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Doctor
{
    public class BaseDoctor
    {
        [Required(ErrorMessage = "Employee is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Medical Diagnosis is required.")]
        public string MedicalDiagnose { get; set; } = string.Empty;

        [Required(ErrorMessage = "Medical Recommendation is required.")]
        public string MedicalRecommendation { get; set; } = string.Empty;

        public string? Other { get; set; }
    }
}

