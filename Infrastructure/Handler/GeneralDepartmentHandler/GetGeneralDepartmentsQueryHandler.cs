using Application.Dtos.Response.GeneralDepartment;
using Application.Service.Contracts;
using Application.Service.GeneralDepartment.Queries;
using AutoMapper;
using DomainGeneralDepartment = Domain.Entities.GeneralDepartment;
using MediatR;

namespace Infrastructure.Handler.GeneralDepartmentHandler
{
    public class GetGeneralDepartmentsQueryHandler : IRequestHandler<GetGeneralDepartmentsQuery, List<GeneralDepartmentResponse>>
    {
        private readonly IGeneralRepositoryInterface<DomainGeneralDepartment> _generalDepartmentRepository;
        private readonly IMapper _mapper;

        public GetGeneralDepartmentsQueryHandler(
            IGeneralRepositoryInterface<DomainGeneralDepartment> generalDepartmentRepository,
            IMapper mapper)
        {
            _generalDepartmentRepository = generalDepartmentRepository;
            _mapper = mapper;
        }

        public async Task<List<GeneralDepartmentResponse>> Handle(
            GetGeneralDepartmentsQuery request,
            CancellationToken cancellationToken)
        {
            var generalDepartments = await _generalDepartmentRepository.GetAllAsync();
            return _mapper.Map<List<GeneralDepartmentResponse>>(generalDepartments);
        }
    }
}
