using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class OverTimeType : BaseEntity
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal RatePerDay { get; set; } = 0m;
    }
}
