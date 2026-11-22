using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Country.Commands;
using DomainCountry = Domain.Entities.Country;
using MediatR;

namespace Infrastructure.Handler.CountryHandler
{
    public class UpdateCountryHandler : IRequestHandler<UpdateCountryCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainCountry> _countryRepository;

        public UpdateCountryHandler(IGeneralRepositoryInterface<DomainCountry> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdateCountryCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Country is null || request.Country.Id <= 0)
                return new GeneralResponse(false, "Invalid Country data or ID.");

            if (string.IsNullOrWhiteSpace(request.Country.Name))
                return new GeneralResponse(false, "Country name is required.");

            var country = await _countryRepository.GetByIdAsync(request.Country.Id);
            if (country is null)
                return new GeneralResponse(false, "Country not found.");

            country.Name = request.Country.Name.Trim();
            return await _countryRepository.UpdateAsync(country);
        }
    }
}
