using Application.Helper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Ensure Database is created / migrated
            await context.Database.MigrateAsync();

            // ── 1. Seed Roles & Default Admin ─────────────────────────────────
            var adminRole = await context.SystemRoles.FirstOrDefaultAsync(r => r.Name == Constants.Admin);
            if (adminRole is null)
            {
                adminRole = new SystemRole { Name = Constants.Admin };
                await context.SystemRoles.AddAsync(adminRole);
                await context.SaveChangesAsync();
            }

            var userRole = await context.SystemRoles.FirstOrDefaultAsync(r => r.Name == Constants.User);
            if (userRole is null)
            {
                userRole = new SystemRole { Name = Constants.User };
                await context.SystemRoles.AddAsync(userRole);
                await context.SaveChangesAsync();
            }

            var adminEmail = "admin@employeesystem.com";
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
            if (adminUser is null)
            {
                adminUser = new ApplicationUser
                {
                    FullName = "System Administrator",
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true
                };
                var hasher = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");
                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();

                var hasAdminRole = await context.UserRoles_App.AnyAsync(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id);
                if (!hasAdminRole)
                {
                    await context.UserRoles_App.AddAsync(new UserRole
                    {
                        RoleId = adminRole.Id,
                        UserId = adminUser.Id
                    });
                    await context.SaveChangesAsync();
                }
            }

            // ── 2. Seed General Departments, Departments & Branches ───────────
            if (!await context.GeneralDepartments.AnyAsync())
            {
                var gdIt = new GeneralDepartment { Name = "Information Technology" };
                var gdHr = new GeneralDepartment { Name = "Human Resources" };
                var gdFinance = new GeneralDepartment { Name = "Finance & Accounting" };
                await context.GeneralDepartments.AddRangeAsync(gdIt, gdHr, gdFinance);
                await context.SaveChangesAsync();

                var depSoftware = new Department { Name = "Software Engineering", GeneralDepartmentId = gdIt.Id };
                var depSupport = new Department { Name = "Technical Support", GeneralDepartmentId = gdIt.Id };
                var depRecruit = new Department { Name = "Recruitment & Onboarding", GeneralDepartmentId = gdHr.Id };
                var depPayroll = new Department { Name = "Payroll & Compensation", GeneralDepartmentId = gdFinance.Id };
                await context.Departments.AddRangeAsync(depSoftware, depSupport, depRecruit, depPayroll);
                await context.SaveChangesAsync();

                var branchHq = new Branch { Name = "Headquarters - Cairo", DepartmentId = depSoftware.Id };
                var branchSmart = new Branch { Name = "Smart Village Branch", DepartmentId = depSoftware.Id };
                var branchAlex = new Branch { Name = "Alexandria Coast Branch", DepartmentId = depRecruit.Id };
                await context.Branches.AddRangeAsync(branchHq, branchSmart, branchAlex);
                await context.SaveChangesAsync();
            }

            // ── 3. Seed Location Hierarchy (Country -> City -> Town) ─────────
            if (!await context.Countries.AnyAsync())
            {
                var countryEgypt = new Country { Name = "Egypt" };
                var countryKsa = new Country { Name = "Saudi Arabia" };
                var countryUae = new Country { Name = "United Arab Emirates" };
                await context.Countries.AddRangeAsync(countryEgypt, countryKsa, countryUae);
                await context.SaveChangesAsync();

                var cityCairo = new City { Name = "Cairo", CountryId = countryEgypt.Id };
                var cityAlex = new City { Name = "Alexandria", CountryId = countryEgypt.Id };
                var cityGiza = new City { Name = "Giza", CountryId = countryEgypt.Id };
                var cityRiyadh = new City { Name = "Riyadh", CountryId = countryKsa.Id };
                await context.Cities.AddRangeAsync(cityCairo, cityAlex, cityGiza, cityRiyadh);
                await context.SaveChangesAsync();

                var townNasrCity = new Town { Name = "Nasr City", CityId = cityCairo.Id };
                var townMaadi = new Town { Name = "Maadi", CityId = cityCairo.Id };
                var townNewCairo = new Town { Name = "New Cairo", CityId = cityCairo.Id };
                var townSmouha = new Town { Name = "Smouha", CityId = cityAlex.Id };
                var townDokki = new Town { Name = "Dokki", CityId = cityGiza.Id };
                await context.Towns.AddRangeAsync(townNasrCity, townMaadi, townNewCairo, townSmouha, townDokki);
                await context.SaveChangesAsync();
            }

            // ── 4. Seed Vacation, Overtime, and Sanction Types ────────────────
            if (!await context.VacationTypes.AnyAsync())
            {
                await context.VacationTypes.AddRangeAsync(
                    new VacationType { Name = "Annual Leave" },
                    new VacationType { Name = "Sick Leave" },
                    new VacationType { Name = "Emergency Leave" },
                    new VacationType { Name = "Maternity Leave" },
                    new VacationType { Name = "Unpaid Leave" }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.OverTimeTypes.AnyAsync())
            {
                await context.OverTimeTypes.AddRangeAsync(
                    new OverTimeType { Name = "Standard Overtime (1.5x)" },
                    new OverTimeType { Name = "Weekend / Holiday Overtime (2.0x)" },
                    new OverTimeType { Name = "Night Shift Overtime" }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.SanctionTypes.AnyAsync())
            {
                await context.SanctionTypes.AddRangeAsync(
                    new SanctionType { Name = "First Verbal Warning" },
                    new SanctionType { Name = "Written Warning" },
                    new SanctionType { Name = "1-Day Salary Deduction" },
                    new SanctionType { Name = "3-Days Salary Deduction" },
                    new SanctionType { Name = "Suspension from Work" }
                );
                await context.SaveChangesAsync();
            }

            // ── 5. Seed Sample Employees with Related Records ─────────────────
            if (!await context.Employees.AnyAsync())
            {
                var branch = await context.Branches.FirstAsync();
                var town = await context.Towns.FirstAsync();
                var vacType = await context.VacationTypes.FirstAsync();
                var otType = await context.OverTimeTypes.FirstAsync();
                var sanctionType = await context.SanctionTypes.FirstAsync();

                var emp1 = new Employee
                {
                    Name = "Ahmed Hassan",
                    Fullname = "Ahmed Hassan Mohamed",
                    CivilId = "29501011234567",
                    FileNumber = "EMP-1001",
                    JobName = "Senior Software Engineer",
                    Address = "15 Abbas El Akkad St, Nasr City",
                    TelephoneNumber = "01012345678",
                    Photo = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=150",
                    BranchId = branch.Id,
                    TownId = town.Id,
                    Other = "Core Architecture Team Lead"
                };

                var emp2 = new Employee
                {
                    Name = "Sara Ibrahim",
                    Fullname = "Sara Ibrahim Mahmoud",
                    CivilId = "29805151234568",
                    FileNumber = "EMP-1002",
                    JobName = "HR Generalist",
                    Address = "22 Road 9, Maadi",
                    TelephoneNumber = "01123456789",
                    Photo = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=150",
                    BranchId = branch.Id,
                    TownId = town.Id,
                    Other = "Talent Acquisition Specialist"
                };

                await context.Employees.AddRangeAsync(emp1, emp2);
                await context.SaveChangesAsync();

                // Related records for Employee 1
                await context.Vacations.AddAsync(new Vacation
                {
                    EmployeeId = emp1.Id,
                    VacationTypeId = vacType.Id,
                    StartData = DateTime.UtcNow.Date.AddDays(-10),
                    NumberOfDays = 5,
                    Other = "Approved by Team Lead"
                });

                await context.OverTimes.AddAsync(new OverTime
                {
                    EmployeeId = emp1.Id,
                    OverTimeTypeId = otType.Id,
                    StartDate = DateTime.UtcNow.Date.AddDays(-3),
                    EndDate = DateTime.UtcNow.Date.AddDays(-1),
                    Other = "Release Deployment Support"
                });

                await context.Doctors.AddAsync(new Doctor
                {
                    EmployeeId = emp1.Id,
                    Data = DateTime.UtcNow.Date.AddMonths(-2),
                    MedicalDiagnose = "Mild Eye Strain",
                    MedicalRecommendation = "Prescription reading glasses and ergonomic screen",
                    Other = "Routine annual health checkup"
                });

                await context.Sanctions.AddAsync(new Sanction
                {
                    EmployeeId = emp1.Id,
                    SanctionTypeId = sanctionType.Id,
                    Data = DateTime.UtcNow.Date.AddMonths(-4),
                    Punishment = "Verbal notice regarding late attendance",
                    PunishmentDate = DateTime.UtcNow.Date.AddMonths(-4),
                    Other = "Resolved amicably"
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
