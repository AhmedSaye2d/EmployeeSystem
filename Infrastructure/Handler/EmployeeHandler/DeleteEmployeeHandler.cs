using Application.Dtos.Response;
using Application.Service.Contracts;
using Application.Service.Employee.Commands;
using Domain.Entities;
using MediatR;

namespace Infrastructure.Handler.EmployeeHandler
{
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, GeneralResponse>
    {
        private readonly IGeneralRepositoryInterface<Employee> _repository;

        public DeleteEmployeeHandler(IGeneralRepositoryInterface<Employee> repository)
        {
            _repository = repository;
        }

        public async Task<GeneralResponse> Handle(
            DeleteEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return new GeneralResponse(false, "Invalid employee ID.");

            return await _repository.DeleteAsync(request.Id);
        }
    }
}
