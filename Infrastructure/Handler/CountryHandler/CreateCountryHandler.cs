using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Country.Commands;
using AutoMapper;
using DomainCountry = Domain.Entities.Country;
using MediatR;

namespace Infrastructure.Handler.CountryHandler
{
    public class CreateCountryHandler : IRequestHandler<CreateCountryCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainCountry> _countryRepository;
        private readonly IMapper _mapper;

        public CreateCountryHandler(
            IGeneralRepositoryInterface<DomainCountry> countryRepository,
            IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(
            CreateCountryCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Country is null)
                return new GeneralResponse(false, "Country data is required.");

            if (string.IsNullOrWhiteSpace(request.Country.Name))
                return new GeneralResponse(false, "Country name is required.");

            var country = _mapper.Map<DomainCountry>(request.Country);
            return await _countryRepository.InsertAsync(country);
        }
    }
}
