using Domain.Entities.Enum;

namespace Application.Dtos.Response.Payroll
{
    public class PayrollResponse
    {
        public int Id { get; set; }

        // Employee Info
        public int EmployeeId { get; set; }
        public string? CivilId { get; set; }
        public string? FileNumber { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? JobName { get; set; }
        public string? Photo { get; set; }
        public string? GeneralDepartmentName { get; set; }
        public string? DepartmentName { get; set; }
        public string? BranchName { get; set; }

        // Period Info
        public int Month { get; set; }
        public int Year { get; set; }

        // Salary Breakdown (Historical Snapshot)
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal OverTimeAmount { get; set; }
        public decimal Deductions { get; set; }
        public decimal GrossSalary => BasicSalary + Allowances + OverTimeAmount;
        public decimal NetSalary { get; set; }

        // Status & Workflow
        public PayrollStatus Status { get; set; }
        public string StatusName => Status.ToString();
        public DateTime CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? Notes { get; set; }
        public string? Warning { get; set; }
    }
}
