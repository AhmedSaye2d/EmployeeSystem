using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Department
{
    public class BaseDepartment
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "General Department is required.")]
        public int GeneralDepartmentId { get; set; }
    }
}

