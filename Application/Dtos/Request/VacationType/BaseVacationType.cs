using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.VacationType
{
    public class BaseVacationType
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}

