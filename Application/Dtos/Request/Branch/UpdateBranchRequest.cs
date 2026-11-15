using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Branch
{
    public class UpdateBranchRequest : BaseBranch
    {
        [Required]
        public int Id { get; set; }
    }
}

