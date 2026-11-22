using Application.Dtos.Response.Doctor;
using Application.Service.Contracts;
using Application.Service.Doctor.Queries;
using AutoMapper;
using MediatR;

namespace Infrastructure.Handler.DoctorHandler
{
    public class GetDoctorsByEmployeeIdQueryHandler : IRequestHandler<GetDoctorsByEmployeeIdQuery, List<DoctorResponse>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public GetDoctorsByEmployeeIdQueryHandler(
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<List<DoctorResponse>> Handle(
            GetDoctorsByEmployeeIdQuery request,
            CancellationToken cancellationToken)
        {
            if (request.EmployeeId <= 0)
                return new List<DoctorResponse>();

            var doctors = await _doctorRepository.GetByEmployeeIdAsync(request.EmployeeId);
            return _mapper.Map<List<DoctorResponse>>(doctors);
        }
    }
}
