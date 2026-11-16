using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.OverTime
{
    public class UpdateOverTimeRequest : BaseOverTime
    {
        [Required]
        public int Id { get; set; }
    }
}

