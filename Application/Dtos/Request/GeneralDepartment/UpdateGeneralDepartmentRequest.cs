using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.GeneralDepartment
{
    public class UpdateGeneralDepartmentRequest : BaseGeneralDepartment
    {
        [Required]
        public int Id { get; set; }
    }
}

