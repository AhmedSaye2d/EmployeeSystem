using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Doctor.Commands;
using AutoMapper;
using Domain.Entities;
using DomainDoctor = Domain.Entities.Doctor;
using MediatR;

namespace Infrastructure.Handler.DoctorHandler
{
    public class CreateDoctorHandler : IRequestHandler<CreateDoctorCommand, GeneralResponse>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public CreateDoctorHandler(
            IDoctorRepository doctorRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> Handle(
            CreateDoctorCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Doctor is null)
                return new GeneralResponse(false, "Doctor data is required.");

            if (string.IsNullOrWhiteSpace(request.Doctor.MedicalDiagnose))
                return new GeneralResponse(false, "Medical Diagnosis is required.");

            if (string.IsNullOrWhiteSpace(request.Doctor.MedicalRecommendation))
                return new GeneralResponse(false, "Medical Recommendation is required.");

            // 1. التحقق من وجود الموظف
            var employee = await _employeeRepository.GetByIdAsync(request.Doctor.EmployeeId);
            if (employee is null)
                return new GeneralResponse(false, "Employee not found.");

            // 2. التحويل والحفظ
            var doctor = _mapper.Map<DomainDoctor>(request.Doctor);
            return await _doctorRepository.InsertAsync(doctor);
        }
    }
}
