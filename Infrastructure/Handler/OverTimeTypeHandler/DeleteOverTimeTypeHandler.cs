using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.OverTimeType.Commands;
using DomainOverTimeType = Domain.Entities.OverTimeType;
using MediatR;

namespace Infrastructure.Handler.OverTimeTypeHandler
{
    public class DeleteOverTimeTypeHandler : IRequestHandler<DeleteOverTimeTypeCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainOverTimeType> _overTimeTypeRepository;

        public DeleteOverTimeTypeHandler(IGeneralRepositoryInterface<DomainOverTimeType> overTimeTypeRepository)
        {
            _overTimeTypeRepository = overTimeTypeRepository;
        }

        public async Task<GeneralResponse> Handle(
            DeleteOverTimeTypeCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return new GeneralResponse(false, "Invalid overtime type ID.");

            return await _overTimeTypeRepository.DeleteAsync(request.Id);
        }
    }
}
