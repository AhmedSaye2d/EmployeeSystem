using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.SanctionType
{
    public class BaseSanctionType
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}

