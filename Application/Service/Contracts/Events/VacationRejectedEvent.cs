namespace Application.Service.Contracts.Events
{
    public record VacationRejectedEvent(
        Guid EventId,
        int VacationId,
        int EmployeeId,
        string EmployeeName,
        string EmployeeEmail,
        string VacationTypeName,
        DateTime StartDate,
        DateTime EndDate,
        int NumberOfDays,
        string RejectedBy,
        DateTime RejectedAt,
        string? RejectionReason
    );
}
