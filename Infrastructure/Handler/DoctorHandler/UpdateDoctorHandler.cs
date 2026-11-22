using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Doctor.Commands;
using Domain.Entities;
using DomainDoctor = Domain.Entities.Doctor;
using MediatR;

namespace Infrastructure.Handler.DoctorHandler
{
    public class UpdateDoctorHandler : IRequestHandler<UpdateDoctorCommand, GeneralResponse>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public UpdateDoctorHandler(
            IDoctorRepository doctorRepository,
            IEmployeeRepository employeeRepository)
        {
            _doctorRepository = doctorRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<GeneralResponse> Handle(
            UpdateDoctorCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Doctor is null || request.Doctor.Id <= 0)
                return new GeneralResponse(false, "Invalid doctor data or ID.");

            if (string.IsNullOrWhiteSpace(request.Doctor.MedicalDiagnose))
                return new GeneralResponse(false, "Medical Diagnosis is required.");

            if (string.IsNullOrWhiteSpace(request.Doctor.MedicalRecommendation))
                return new GeneralResponse(false, "Medical Recommendation is required.");

            // 1. التحقق من وجود سجل الفحص الطبي
            var doctor = await _doctorRepository.GetByIdAsync(request.Doctor.Id);
            if (doctor is null)
                return new GeneralResponse(false, "Medical record not found.");

            // 2. التحقق من وجود الموظف
            var employee = await _employeeRepository.GetByIdAsync(request.Doctor.EmployeeId);
            if (employee is null)
                return new GeneralResponse(false, "Employee not found.");

            // 3. تحديث البيانات
            doctor.EmployeeId            = request.Doctor.EmployeeId;
            doctor.Data                  = request.Doctor.Data;
            doctor.MedicalDiagnose       = request.Doctor.MedicalDiagnose.Trim();
            doctor.MedicalRecommendation = request.Doctor.MedicalRecommendation.Trim();
            doctor.Other                 = request.Doctor.Other;

            return await _doctorRepository.UpdateAsync(doctor);
        }
    }
}
