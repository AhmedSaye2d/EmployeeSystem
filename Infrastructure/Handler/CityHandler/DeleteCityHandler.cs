using Application.Dtos.Response;
using Application.Service.City.Commands;
using Application.Service.Contracts;
using DomainCity = Domain.Entities.City;
using MediatR;

namespace Infrastructure.Handler.CityHandler
{
    public class DeleteCityHandler : IRequestHandler<DeleteCityCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainCity> _cityRepository;

        public DeleteCityHandler(IGeneralRepositoryInterface<DomainCity> cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<GeneralResponse> Handle(
            DeleteCityCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return new GeneralResponse(false, "Invalid City ID.");

            return await _cityRepository.DeleteAsync(request.Id);
        }
    }
}
