using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Country
{
    public class BaseCountry
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}

