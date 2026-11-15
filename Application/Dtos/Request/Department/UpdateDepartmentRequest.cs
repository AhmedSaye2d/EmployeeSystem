using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Department
{
    public class UpdateDepartmentRequest : BaseDepartment
    {
        [Required]
        public int Id { get; set; }
    }
}

