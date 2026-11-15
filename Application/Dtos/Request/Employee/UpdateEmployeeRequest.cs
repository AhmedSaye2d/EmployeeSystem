using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Employee
{
    public class UpdateEmployeeRequest : BaseEmployee
    {
        [Required]
        public int Id { get; set; }
    }
}

