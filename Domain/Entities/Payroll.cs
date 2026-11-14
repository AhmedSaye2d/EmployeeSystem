using Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Payroll : OtherBaseEntity
    {
        // ── فترة المسير ────────────────────────────────────────────────
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        public int Month { get; set; }

        [Range(2000, 2100, ErrorMessage = "Year must be a valid year.")]
        public int Year { get; set; }

        // ── عناصر الراتب المحفوظة كلقطة تاريخية (Historical Snapshot) ─
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Allowances { get; set; } = 0m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal OverTimeAmount { get; set; } = 0m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Deductions { get; set; } = 0m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetSalary { get; set; }

        // ── الحالة ودورة الحياة ─────────────────────────────────────────
        public PayrollStatus Status { get; set; } = PayrollStatus.Draft;

        // تواريخ التتبع (فصل الإنشاء عن الاعتماد عن الصرف)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ApprovedAt { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string? Notes { get; set; }
    }
}
