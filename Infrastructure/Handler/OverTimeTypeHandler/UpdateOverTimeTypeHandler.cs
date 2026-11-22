using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.OverTimeType.Commands;
using DomainOverTimeType = Domain.Entities.OverTimeType;
using MediatR;

namespace Infrastructure.Handler.OverTimeTypeHandler
{
    public class UpdateOverTimeTypeHandler : IRequestHandler<UpdateOverTimeTypeCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainOverTimeType> _overTimeTypeRepository;

        public UpdateOverTimeTypeHandler(IGeneralRepositoryInterface<DomainOverTimeType> overTimeTypeRepository)
        {
            _overTimeTypeRepository = overTimeTypeRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdateOverTimeTypeCommand request,
            CancellationToken cancellationToken)
        {
            if (request.OverTimeType is null || request.OverTimeType.Id <= 0)
                return new GeneralResponse(false, "Invalid overtime type data or ID.");

            if (string.IsNullOrWhiteSpace(request.OverTimeType.Name))
                return new GeneralResponse(false, "Overtime type name is required.");

            var overTimeType = await _overTimeTypeRepository.GetByIdAsync(request.OverTimeType.Id);
            if (overTimeType is null)
                return new GeneralResponse(false, "Overtime type not found.");

            overTimeType.Name = request.OverTimeType.Name.Trim();
            return await _overTimeTypeRepository.UpdateAsync(overTimeType);
        }
    }
}
