using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.OverTimeType
{
    public class BaseOverTimeType
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}

