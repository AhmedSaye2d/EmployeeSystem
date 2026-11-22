using Application.Dtos.Response;
using Application.Service.City.Commands;
using Application.Service.Contracts;
using AutoMapper;
using Domain.Entities;
using DomainCity = Domain.Entities.City;
using MediatR;

namespace Infrastructure.Handler.CityHandler
{
    public class CreateCityHandler : IRequestHandler<CreateCityCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainCity> _cityRepository;
        private readonly IGeneralRepositoryInterface<Country> _countryRepository;
        private readonly IMapper _mapper;

        public CreateCityHandler(
            IGeneralRepositoryInterface<DomainCity> cityRepository,
            IGeneralRepositoryInterface<Country> countryRepository,
            IMapper mapper)
        {
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(
            CreateCityCommand request,
            CancellationToken cancellationToken)
        {
            if (request.City is null)
                return new GeneralResponse(false, "City data is required.");

            if (string.IsNullOrWhiteSpace(request.City.Name))
                return new GeneralResponse(false, "City name is required.");

            if (request.City.CountryId <= 0)
                return new GeneralResponse(false, "Country is required.");

            // 1. التحقق من وجود الدولة
            var country = await _countryRepository.GetByIdAsync(request.City.CountryId);
            if (country is null)
                return new GeneralResponse(false, "Selected Country does not exist.");

            // 2. التحويل والحفظ
            var city = _mapper.Map<DomainCity>(request.City);
            return await _cityRepository.InsertAsync(city);
        }
    }
}
