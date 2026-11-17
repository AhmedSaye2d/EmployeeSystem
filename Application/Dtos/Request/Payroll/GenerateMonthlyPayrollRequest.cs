using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Payroll
{
    public class GenerateMonthlyPayrollRequest
    {
        [Required(ErrorMessage = "Month is required.")]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(2000, 2100, ErrorMessage = "Year must be a valid year.")]
        public int Year { get; set; }

        /// <summary>
        /// Optional list of specific Employee IDs. If null or empty, payroll is generated for all active employees.
        /// </summary>
        public List<int>? EmployeeIds { get; set; }

        public string? Notes { get; set; }
    }
}
