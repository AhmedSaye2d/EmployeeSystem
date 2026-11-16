using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Country
{
    public class UpdateCountryRequest : BaseCountry
    {
        [Required]
        public int Id { get; set; }
    }
}

