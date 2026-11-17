namespace Application.Service.Contracts
{
    public interface IPayrollCalculator
    {
        Task<(decimal OverTimeAmount, decimal Deductions, decimal NetSalary, string? Warning)> CalculateAsync(
            int employeeId,
            decimal basicSalary,
            decimal allowances,
            int month,
            int year,
            CancellationToken cancellationToken = default);
    }
}
