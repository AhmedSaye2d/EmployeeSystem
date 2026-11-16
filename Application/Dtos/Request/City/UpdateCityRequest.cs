using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.City
{
    public class UpdateCityRequest : BaseCity
    {
        [Required]
        public int Id { get; set; }
    }
}

