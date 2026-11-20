using Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            // ── Payroll Configurations ────────────────────────────────────
            modelBuilder.Entity<Payroll>(entity =>
            {
                entity.HasIndex(p => new { p.EmployeeId, p.Month, p.Year })
                      .IsUnique();

                entity.HasOne(p => p.Employee)
                      .WithMany(e => e.Payrolls)
                      .HasForeignKey(p => p.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.BasicSalary).HasPrecision(18, 2);
                entity.Property(p => p.Allowances).HasPrecision(18, 2);
                entity.Property(p => p.OverTimeAmount).HasPrecision(18, 2);
                entity.Property(p => p.Deductions).HasPrecision(18, 2);
                entity.Property(p => p.NetSalary).HasPrecision(18, 2);
            });

            // ── Precision Configurations for decimals ─────────────────────
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.BasicSalary).HasPrecision(18, 2);
            });

            modelBuilder.Entity<OverTimeType>(entity =>
            {
                entity.Property(ot => ot.RatePerDay).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Sanction>(entity =>
            {
                entity.Property(s => s.DeductionAmount).HasPrecision(18, 2);
            });

            // ── Idempotency Configurations ────────────────────────────────
            modelBuilder.Entity<ProcessedMessage>(entity =>
            {
                entity.ToTable("ProcessedMessages");
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.EventId).IsUnique();
                entity.Property(p => p.MessageType).HasMaxLength(250).IsRequired();
                entity.Property(p => p.ConsumerName).HasMaxLength(250).IsRequired();
            });
        }

        // ── Organisational ──────────────────────────────────────────
        public DbSet<GeneralDepartment> GeneralDepartments { get; set; }
        public DbSet<Department>        Departments        { get; set; }
        public DbSet<Branch>            Branches           { get; set; }

        // ── Employee ─────────────────────────────────────────────────
        public DbSet<Employee>          Employees          { get; set; }

        // ── Employee records ─────────────────────────────────────────
        public DbSet<Vacation>          Vacations          { get; set; }
        public DbSet<VacationType>      VacationTypes      { get; set; }
        public DbSet<OverTime>          OverTimes          { get; set; }
        public DbSet<OverTimeType>      OverTimeTypes      { get; set; }
        public DbSet<Doctor>            Doctors            { get; set; }
        public DbSet<Sanction>          Sanctions          { get; set; }
        public DbSet<SanctionType>      SanctionTypes      { get; set; }
        public DbSet<Payroll>           Payrolls           { get; set; }
        public DbSet<ProcessedMessage>  ProcessedMessages  { get; set; }

        // ── Location ─────────────────────────────────────────────────
        public DbSet<Country>           Countries          { get; set; }
        public DbSet<City>              Cities             { get; set; }
        public DbSet<Town>              Towns              { get; set; }

        // ── Auth / Roles ──────────────────────────────────────────────
        public DbSet<SystemRole>        SystemRoles        { get; set; }
        public DbSet<UserRole>          UserRoles_App      { get; set; }
        public DbSet<RefreshTokenInfo>  RefreshTokenInfos  { get; set; }
    }
}
