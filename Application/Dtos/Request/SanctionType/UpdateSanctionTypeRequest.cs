using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.SanctionType
{
    public class UpdateSanctionTypeRequest : BaseSanctionType
    {
        [Required]
        public int Id { get; set; }
    }
}

