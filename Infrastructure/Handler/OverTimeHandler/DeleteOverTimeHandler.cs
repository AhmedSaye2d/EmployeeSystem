using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.OverTime.Commands;
using MediatR;

namespace Infrastructure.Handler.OverTimeHandler
{
    public class DeleteOverTimeHandler : IRequestHandler<DeleteOverTimeCommand, GeneralResponse>
    {
        private readonly IOverTimeRepository _overTimeRepository;

        public DeleteOverTimeHandler(IOverTimeRepository overTimeRepository)
        {
            _overTimeRepository = overTimeRepository;
        }

        public async Task<GeneralResponse> Handle(
            DeleteOverTimeCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return new GeneralResponse(false, "Invalid overtime ID.");

            return await _overTimeRepository.DeleteAsync(request.Id);
        }
    }
}
