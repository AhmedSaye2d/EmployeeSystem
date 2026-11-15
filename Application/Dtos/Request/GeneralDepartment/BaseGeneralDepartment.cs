using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.GeneralDepartment
{
    public class BaseGeneralDepartment
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
    }
}

