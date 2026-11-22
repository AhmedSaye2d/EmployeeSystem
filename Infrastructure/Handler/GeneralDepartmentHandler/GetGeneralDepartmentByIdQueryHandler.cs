using Application.Dtos.Response.GeneralDepartment;
using Application.Service.Contracts;
using Application.Service.GeneralDepartment.Queries;
using AutoMapper;
using DomainGeneralDepartment = Domain.Entities.GeneralDepartment;
using MediatR;

namespace Infrastructure.Handler.GeneralDepartmentHandler
{
    public class GetGeneralDepartmentByIdQueryHandler : IRequestHandler<GetGeneralDepartmentByIdQuery, GeneralDepartmentResponse?>
    {
        private readonly IGeneralRepositoryInterface<DomainGeneralDepartment> _generalDepartmentRepository;
        private readonly IMapper _mapper;

        public GetGeneralDepartmentByIdQueryHandler(
            IGeneralRepositoryInterface<DomainGeneralDepartment> generalDepartmentRepository,
            IMapper mapper)
        {
            _generalDepartmentRepository = generalDepartmentRepository;
            _mapper = mapper;
        }

        public async Task<GeneralDepartmentResponse?> Handle(
            GetGeneralDepartmentByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var generalDepartment = await _generalDepartmentRepository.GetByIdAsync(request.Id);
            return generalDepartment is null ? null : _mapper.Map<GeneralDepartmentResponse>(generalDepartment);
        }
    }
}
