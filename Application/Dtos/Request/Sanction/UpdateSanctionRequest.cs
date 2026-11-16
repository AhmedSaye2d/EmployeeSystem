using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Sanction
{
    public class UpdateSanctionRequest : BaseSanction
    {
        [Required]
        public int Id { get; set; }
    }
}

