namespace Application.Service.Contracts.Events
{
    public record VacationApprovedEvent(
        Guid EventId,
        int VacationId,
        int EmployeeId,
        string EmployeeName,
        string EmployeeEmail,
        string VacationTypeName,
        DateTime StartDate,
        DateTime EndDate,
        int NumberOfDays,
        string ApprovedBy,
        DateTime ApprovedAt
    );
}
