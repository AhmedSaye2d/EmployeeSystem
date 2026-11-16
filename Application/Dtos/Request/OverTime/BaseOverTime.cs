using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.OverTime
{
    public class BaseOverTime
    {
        [Required(ErrorMessage = "Employee is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Overtime Type is required.")]
        public int OverTimeTypeId { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        public DateTime EndDate { get; set; }

        public string? Other { get; set; }
    }
}

