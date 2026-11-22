using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Country.Commands;
using DomainCountry = Domain.Entities.Country;
using MediatR;

namespace Infrastructure.Handler.CountryHandler
{
    public class DeleteCountryHandler : IRequestHandler<DeleteCountryCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainCountry> _countryRepository;

        public DeleteCountryHandler(IGeneralRepositoryInterface<DomainCountry> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<GeneralResponse> Handle(
            DeleteCountryCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return new GeneralResponse(false, "Invalid Country ID.");

            return await _countryRepository.DeleteAsync(request.Id);
        }
    }
}
