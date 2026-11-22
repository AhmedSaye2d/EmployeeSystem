using Application.Dtos.Response;
using Application.Service.City.Commands;
using Application.Service.Contracts;
using Domain.Entities;
using DomainCity = Domain.Entities.City;
using MediatR;

namespace Infrastructure.Handler.CityHandler
{
    public class UpdateCityHandler : IRequestHandler<UpdateCityCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainCity> _cityRepository;
        private readonly IGeneralRepositoryInterface<Country> _countryRepository;

        public UpdateCityHandler(
            IGeneralRepositoryInterface<DomainCity> cityRepository,
            IGeneralRepositoryInterface<Country> countryRepository)
        {
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdateCityCommand request,
            CancellationToken cancellationToken)
        {
            if (request.City is null || request.City.Id <= 0)
                return new GeneralResponse(false, "Invalid City data or ID.");

            if (string.IsNullOrWhiteSpace(request.City.Name))
                return new GeneralResponse(false, "City name is required.");

            if (request.City.CountryId <= 0)
                return new GeneralResponse(false, "Country is required.");

            // 1. التحقق من وجود المدينة
            var city = await _cityRepository.GetByIdAsync(request.City.Id);
            if (city is null)
                return new GeneralResponse(false, "City not found.");

            // 2. التحقق من وجود الدولة
            var country = await _countryRepository.GetByIdAsync(request.City.CountryId);
            if (country is null)
                return new GeneralResponse(false, "Selected Country does not exist.");

            // 3. تحديث البيانات
            city.Name = request.City.Name.Trim();
            city.CountryId = request.City.CountryId;

            return await _cityRepository.UpdateAsync(city);
        }
    }
}
