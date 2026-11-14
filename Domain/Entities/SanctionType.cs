using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("SnactionTypes")]
    public class SanctionType : BaseEntity
    {
        public List<Sanction> Sanctions { get; set; } = new List<Sanction>();
    }
}
