using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Payroll
{
    public class UpdatePayrollDraftRequest
    {
        [Required]
        public int Id { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Allowances must be non-negative.")]
        public decimal Allowances { get; set; }

        public string? Notes { get; set; }
    }
}
