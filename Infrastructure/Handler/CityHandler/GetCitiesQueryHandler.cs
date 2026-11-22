using Application.Dtos.Response.City;
using Application.Service.City.Queries;
using Application.Service.Contracts;
using AutoMapper;
using DomainCity = Domain.Entities.City;
using MediatR;

namespace Infrastructure.Handler.CityHandler
{
    public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, List<CityResponse>>
    {
        private readonly IGeneralRepositoryInterface<DomainCity> _cityRepository;
        private readonly IMapper _mapper;

        public GetCitiesQueryHandler(
            IGeneralRepositoryInterface<DomainCity> cityRepository,
            IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<List<CityResponse>> Handle(
            GetCitiesQuery request,
            CancellationToken cancellationToken)
        {
            var cities = await _cityRepository.GetAllAsync();
            return _mapper.Map<List<CityResponse>>(cities);
        }
    }
}
