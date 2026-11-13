using Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Vacation : OtherBaseEntity
    {
        public DateTime StartData { get; set; }
        public DateTime EndData => NumberOfDays > 0 ? StartData.AddDays(NumberOfDays - 1) : StartData;
        public int NumberOfDays { get; set; }
        [Required]
        public int VacationTypeId { get; set; }
        public VacationType VacationType { get; set; } = null!;

        public string? VacationTypeName => VacationType?.Name;

        // ── Workflow Properties ──────────────────────────────────────────
        public VacationStatus Status { get; set; } = VacationStatus.Pending;
        public string? ApprovedOrRejectedBy { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? DecisionReason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
