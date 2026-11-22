using Application.Dtos.Response.Doctor;
using Application.Service.Contracts;
using Application.Service.Doctor.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.DoctorHandler
{
    public class GetDoctorsQueryHandler : IRequestHandler<GetDoctorsQuery, List<DoctorResponse>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public GetDoctorsQueryHandler(
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<List<DoctorResponse>> Handle(
            GetDoctorsQuery request,
            CancellationToken cancellationToken)
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return _mapper.Map<List<DoctorResponse>>(doctors);
        }
    }
}
