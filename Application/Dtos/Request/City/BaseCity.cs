using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.City
{
    public class BaseCity
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }
    }
}

