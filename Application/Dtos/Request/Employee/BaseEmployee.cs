using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Employee
{
    public class BaseEmployee
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Civil ID is required.")]
        public string CivilId { get; set; } = string.Empty;

        [Required(ErrorMessage = "File Number is required.")]
        public string FileNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required.")]
        public string Fullname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Job Name is required.")]
        public string JobName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telephone Number is required.")]
        public string TelephoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Photo is required.")]
        public string Photo { get; set; } = string.Empty;

        public string? Other { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Branch is required.")]
        public int BranchId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Branch is required.")]
        public int TownId { get; set; }
    }
}

