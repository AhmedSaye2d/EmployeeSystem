using Application.Dtos.Request.Branch;
using Application.Dtos.Response.Branch;
using AutoMapper;
using DomainBranch = Domain.Entities.Branch;

namespace Application.Mapping
{
    public class BranchMappingProfile : Profile
    {
        public BranchMappingProfile()
        {
            CreateMap<DomainBranch, BranchResponse>()
                .ForMember(d => d.DepartmentName, opt => opt.MapFrom(s => s.Department != null ? s.Department.Name : null));

            CreateMap<AddBranchRequest, DomainBranch>();
            CreateMap<UpdateBranchRequest, DomainBranch>();
        }
    }
}
