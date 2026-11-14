using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class OverTime : OtherBaseEntity
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int OverTimeTypeId { get; set; }
        public int NumberOfDays => EndDate.Date >= StartDate.Date ? (EndDate.Date - StartDate.Date).Days + 1 : 0;
        public OverTimeType OverTimeType { get; set; } = null!;

        public string? OverTimeTypeName => OverTimeType?.Name;
    }
}
