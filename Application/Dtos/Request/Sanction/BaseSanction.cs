using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Sanction
{
    public class BaseSanction
    {
        [Required(ErrorMessage = "Employee is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Sanction Type is required.")]
        public int SanctionTypeId { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Punishment description is required.")]
        public string Punishment { get; set; } = string.Empty;

        [Required(ErrorMessage = "Punishment Date is required.")]
        public DateTime PunishmentDate { get; set; }

        public string? Other { get; set; }
    }
}

