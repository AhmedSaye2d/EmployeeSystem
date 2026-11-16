using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Vacation
{
    public class BaseVacation
    {
        [Required(ErrorMessage = "Employee is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Vacation Type is required.")]
        public int VacationTypeId { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartData { get; set; }

        [Required(ErrorMessage = "Number of Days is required.")]
        [Range(1, 365, ErrorMessage = "Number of days must be at least 1.")]
        public int NumberOfDays { get; set; }

        public string? Other { get; set; }
    }
}

