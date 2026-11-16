using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Vacation
{
    public class UpdateVacationRequest : BaseVacation
    {
        [Required]
        public int Id { get; set; }
    }
}

