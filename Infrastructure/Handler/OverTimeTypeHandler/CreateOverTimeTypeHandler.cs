using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.OverTimeType.Commands;
using AutoMapper;
using DomainOverTimeType = Domain.Entities.OverTimeType;
using MediatR;

namespace Infrastructure.Handler.OverTimeTypeHandler
{
    public class CreateOverTimeTypeHandler : IRequestHandler<CreateOverTimeTypeCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainOverTimeType> _overTimeTypeRepository;
        private readonly IMapper _mapper;

        public CreateOverTimeTypeHandler(
            IGeneralRepositoryInterface<DomainOverTimeType> overTimeTypeRepository,
            IMapper mapper)
        {
            _overTimeTypeRepository = overTimeTypeRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(
            CreateOverTimeTypeCommand request,
            CancellationToken cancellationToken)
        {
            if (request.OverTimeType is null)
                return new GeneralResponse(false, "Overtime type data is required.");

            if (string.IsNullOrWhiteSpace(request.OverTimeType.Name))
                return new GeneralResponse(false, "Overtime type name is required.");

            var overTimeType = _mapper.Map<DomainOverTimeType>(request.OverTimeType);
            return await _overTimeTypeRepository.InsertAsync(overTimeType);
        }
    }
}
