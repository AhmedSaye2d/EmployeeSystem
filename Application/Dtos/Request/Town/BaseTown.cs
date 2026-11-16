using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Town
{
    public class BaseTown
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public int CityId { get; set; }
    }
}
