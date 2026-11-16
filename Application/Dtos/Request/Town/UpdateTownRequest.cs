using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Town
{
    public class UpdateTownRequest : BaseTown
    {
        [Required]
        public int Id { get; set; }
    }
}

