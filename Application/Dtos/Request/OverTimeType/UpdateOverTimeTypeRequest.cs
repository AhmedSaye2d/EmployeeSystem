using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.OverTimeType
{
    public class UpdateOverTimeTypeRequest : BaseOverTimeType
    {
        [Required]
        public int Id { get; set; }
    }
}

