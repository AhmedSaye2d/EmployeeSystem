using Application.Dtos.Response.Country;
using Application.Service.Contracts;
using Application.Service.Country.Queries;
using AutoMapper;
using DomainCountry = Domain.Entities.Country;
using MediatR;

namespace Infrastructure.Handler.CountryHandler
{
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, List<CountryResponse>>
    {
        private readonly IGeneralRepositoryInterface<DomainCountry> _countryRepository;
        private readonly IMapper _mapper;

        public GetCountriesQueryHandler(
            IGeneralRepositoryInterface<DomainCountry> countryRepository,
            IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<List<CountryResponse>> Handle(
            GetCountriesQuery request,
            CancellationToken cancellationToken)
        {
            var countries = await _countryRepository.GetAllAsync();
            return _mapper.Map<List<CountryResponse>>(countries);
        }
    }
}
