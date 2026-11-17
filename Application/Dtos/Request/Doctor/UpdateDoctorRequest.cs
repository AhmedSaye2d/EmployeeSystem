using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Doctor
{
    public class UpdateDoctorRequest : BaseDoctor
    {
        [Required]
        public int Id { get; set; }
    }
}

