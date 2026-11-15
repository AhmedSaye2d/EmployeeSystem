using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Branch
{
    public class BaseBranch
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "General Department is required.")]
        public int GeneralDepartmentId { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public int DepartmentId { get; set; }
    }
}
