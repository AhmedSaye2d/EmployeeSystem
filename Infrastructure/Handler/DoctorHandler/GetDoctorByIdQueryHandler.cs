using Application.Dtos.Response.Doctor;
using Application.Service.Contracts;
using Application.Service.Doctor.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.DoctorHandler
{
    public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, DoctorResponse?>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public GetDoctorByIdQueryHandler(
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<DoctorResponse?> Handle(
            GetDoctorByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return null;

            var doctor = await _doctorRepository.GetByIdAsync(request.Id);
            return doctor is null ? null : _mapper.Map<DoctorResponse>(doctor);
        }
    }
}
