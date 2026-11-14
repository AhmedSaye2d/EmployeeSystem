using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Snactions")]
    public class Sanction : OtherBaseEntity
    {
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public string? Punishment { get; set; }
        [Required]
        public DateTime PunishmentDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DeductionAmount { get; set; } = 0m;

        // FK → SanctionType
        [Column("SnactionTypeId")]
        public int SanctionTypeId { get; set; }
        public SanctionType SanctionType { get; set; } = null!;

        public string? SanctionTypeName => SanctionType?.Name;
    }
}
