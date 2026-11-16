using Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Vacation
{
    public class ReviewVacationRequest
    {
        [Required(ErrorMessage = "Vacation ID is required.")]
        public int VacationId { get; set; }

        [Required(ErrorMessage = "Decision is required (2 = Approved, 3 = Rejected).")]
        public VacationStatus Decision { get; set; }

        public string? Reason { get; set; }
    }
}
