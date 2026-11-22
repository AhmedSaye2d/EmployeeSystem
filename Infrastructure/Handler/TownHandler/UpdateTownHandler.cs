using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Town.Commands;
using Domain.Entities;
using DomainTown = Domain.Entities.Town;
using MediatR;

namespace Infrastructure.Handler.TownHandler
{
    public class UpdateTownHandler : IRequestHandler<UpdateTownCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainTown> _townRepository;
        private readonly IGeneralRepositoryInterface<City> _cityRepository;

        public UpdateTownHandler(
            IGeneralRepositoryInterface<DomainTown> townRepository,
            IGeneralRepositoryInterface<City> cityRepository)
        {
            _townRepository = townRepository;
            _cityRepository = cityRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdateTownCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Town is null || request.Town.Id <= 0)
                return new GeneralResponse(false, "Invalid Town data or ID.");

            if (string.IsNullOrWhiteSpace(request.Town.Name))
                return new GeneralResponse(false, "Town name is required.");

            if (request.Town.CountryId <= 0)
                return new GeneralResponse(false, "Country is required.");

            if (request.Town.CityId <= 0)
                return new GeneralResponse(false, "City is required.");

            // 1. التحقق من وجود البلدة
            var town = await _townRepository.GetByIdAsync(request.Town.Id);
            if (town is null)
                return new GeneralResponse(false, "Town not found.");

            // 2. التحقق من وجود المدينة
            var city = await _cityRepository.GetByIdAsync(request.Town.CityId);
            if (city is null)
                return new GeneralResponse(false, "Selected City does not exist.");

            // 3. التحقق إن المدينة تتبع الدولة المحددة
            if (city.CountryId != request.Town.CountryId)
                return new GeneralResponse(false, "Selected City does not belong to the selected Country.");

            // 4. تحديث البيانات
            town.Name = request.Town.Name.Trim();
            town.CityId = request.Town.CityId;

            return await _townRepository.UpdateAsync(town);
        }
    }
}
