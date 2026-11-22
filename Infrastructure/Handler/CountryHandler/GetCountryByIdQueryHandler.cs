using Application.Dtos.Response.Country;
using Application.Service.Contracts;
using Application.Service.Country.Queries;
using AutoMapper;
using DomainCountry = Domain.Entities.Country;
using MediatR;

namespace Infrastructure.Handler.CountryHandler
{
    public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, CountryResponse?>
    {
        private readonly IGeneralRepositoryInterface<DomainCountry> _countryRepository;
        private readonly IMapper _mapper;

        public GetCountryByIdQueryHandler(
            IGeneralRepositoryInterface<DomainCountry> countryRepository,
            IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<CountryResponse?> Handle(
            GetCountryByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var country = await _countryRepository.GetByIdAsync(request.Id);
            return country is null ? null : _mapper.Map<CountryResponse>(country);
        }
    }
}
