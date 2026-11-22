using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Town.Commands;
using AutoMapper;
using Domain.Entities;
using DomainTown = Domain.Entities.Town;
using MediatR;

namespace Infrastructure.Handler.TownHandler
{
    public class CreateTownHandler : IRequestHandler<CreateTownCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<DomainTown> _townRepository;
        private readonly IGeneralRepositoryInterface<City> _cityRepository;
        private readonly IMapper _mapper;

        public CreateTownHandler(
            IGeneralRepositoryInterface<DomainTown> townRepository,
            IGeneralRepositoryInterface<City> cityRepository,
            IMapper mapper)
        {
            _townRepository = townRepository;
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(
            CreateTownCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Town is null)
                return new GeneralResponse(false, "Town data is required.");

            if (string.IsNullOrWhiteSpace(request.Town.Name))
                return new GeneralResponse(false, "Town name is required.");

            if (request.Town.CountryId <= 0)
                return new GeneralResponse(false, "Country is required.");

            if (request.Town.CityId <= 0)
                return new GeneralResponse(false, "City is required.");

            // 1. التحقق من وجود المدينة
            var city = await _cityRepository.GetByIdAsync(request.Town.CityId);
            if (city is null)
                return new GeneralResponse(false, "Selected City does not exist.");

            // 2. التحقق إن المدينة تتبع الدولة المحددة
            if (city.CountryId != request.Town.CountryId)
                return new GeneralResponse(false, "Selected City does not belong to the selected Country.");

            // 3. التحويل والحفظ
            var town = _mapper.Map<DomainTown>(request.Town);
            return await _townRepository.InsertAsync(town);
        }
    }
}
