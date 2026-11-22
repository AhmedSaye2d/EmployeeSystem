using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Sanction.Commands;
using Domain.Entities;
using DomainSanction = Domain.Entities.Sanction;
using MediatR;

namespace Infrastructure.Handler.SanctionHandler
{
    public class DeleteSanctionHandler : IRequestHandler<DeleteSanctionCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainSanction> _sanctionRepository;

        public DeleteSanctionHandler(IGeneralRepositoryInterface<DomainSanction> sanctionRepository)
        {
            _sanctionRepository = sanctionRepository;
        }

        public async Task<GeneralResponse> Handle(
            DeleteSanctionCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return new GeneralResponse(false, "Invalid sanction ID.");

            return await _sanctionRepository.DeleteAsync(request.Id);
        }
    }
}
