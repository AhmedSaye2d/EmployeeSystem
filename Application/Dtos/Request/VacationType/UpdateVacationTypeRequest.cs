using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.VacationType
{
    public class UpdateVacationTypeRequest : BaseVacationType
    {
        [Required]
        public int Id { get; set; }
    }
}

