using Application.Dtos.Response.City;
using Application.Service.City.Queries;
using Application.Service.Contracts;
using AutoMapper;
using DomainCity = Domain.Entities.City;
using MediatR;

namespace Infrastructure.Handler.CityHandler
{
    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityResponse?>
    {
        private readonly IGeneralRepositoryInterface<DomainCity> _cityRepository;
        private readonly IMapper _mapper;

        public GetCityByIdQueryHandler(
            IGeneralRepositoryInterface<DomainCity> cityRepository,
            IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<CityResponse?> Handle(
            GetCityByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var city = await _cityRepository.GetByIdAsync(request.Id);
            return city is null ? null : _mapper.Map<CityResponse>(city);
        }
    }
}
