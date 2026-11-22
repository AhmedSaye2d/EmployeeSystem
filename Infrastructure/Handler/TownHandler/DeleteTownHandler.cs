using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Town.Commands;
using DomainTown = Domain.Entities.Town;
using MediatR;

namespace Infrastructure.Handler.TownHandler
{
    public class DeleteTownHandler : IRequestHandler<DeleteTownCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainTown> _townRepository;

        public DeleteTownHandler(IGeneralRepositoryInterface<DomainTown> townRepository)
        {
            _townRepository = townRepository;
        }

        public async Task<GeneralResponse> Handle(
            DeleteTownCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return new GeneralResponse(false, "Invalid Town ID.");

            return await _townRepository.DeleteAsync(request.Id);
        }
    }
}
