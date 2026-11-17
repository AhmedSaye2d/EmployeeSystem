using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Payroll
{
    public class BasePayrollRequest
    {
        [Required(ErrorMessage = "Employee is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Month is required.")]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(2000, 2100, ErrorMessage = "Year must be a valid year.")]
        public int Year { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Allowances must be non-negative.")]
        public decimal Allowances { get; set; } = 0m;

        public string? Notes { get; set; }
    }
}
