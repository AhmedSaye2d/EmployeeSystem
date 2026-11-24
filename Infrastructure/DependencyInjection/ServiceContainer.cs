using Application.Helper;
using Application.Service.Contracts;
using Application.Service.Contracts.Email;
using Application.Service.Contracts.Logging;
using Domain.Entities;
using Infrastructure.Consumers;
using Infrastructure.DataAccess;
using Infrastructure.Middleware;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Email;
using Infrastructure.Services.Payroll;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ── Database ──────────────────────────────────────────────────────────
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // ── JWT & Authentication ──────────────────────────────────────────────
            services.Configure<JwtSection>(configuration.GetSection("JwtSection"));
            var jwtSection = configuration.GetSection("JwtSection").Get<JwtSection>()
                             ?? configuration.GetSection("jwtSection").Get<JwtSection>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSection?.Issuer,
                    ValidAudience = jwtSection?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSection?.Key ?? "DefaultSecretKey12345678901234567890"))
                };
            });

            // ── Authentication Service ────────────────────────────────────────────
            services.AddScoped<IUserAccount, AuthenticationService>();

            // ── AutoMapper ────────────────────────────────────────────────────────
            services.AddAutoMapper(typeof(Application.Mapping.EmployeeMappingProfile).Assembly);

            // ── MediatR ───────────────────────────────────────────────────────────
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(Application.Service.Employee.Commands.CreateEmployeeCommand).Assembly,
                    typeof(Infrastructure.Handler.EmployeeHandler.CreateEmployeeHandler).Assembly);
            });

            // ── Logging ───────────────────────────────────────────────────────────
            services.AddScoped(typeof(IAppLogger<>), typeof(SerilogLoggerAdapter<>));

            // ── Email Service ─────────────────────────────────────────────────────
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailService, EmailService>();

            // ── Idempotency Service ───────────────────────────────────────────────
            services.AddScoped<Application.Service.Contracts.Idempotency.IIdempotentMessageService, Infrastructure.Services.Idempotency.IdempotentMessageService>();

            // ── RabbitMQ & MassTransit with Transactional Outbox ─────────────────
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
            var rabbitSettings = configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>()
                                 ?? new RabbitMqSettings();

            services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<AppDbContext>(o =>
                {
                    o.UseSqlServer();
                    o.UseBusOutbox();
                });

                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumers(typeof(ServiceContainer).Assembly);

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitSettings.Host, (ushort)rabbitSettings.Port, rabbitSettings.VirtualHost, h =>
                    {
                        h.Username(rabbitSettings.Username);
                        h.Password(rabbitSettings.Password);
                    });

                    // Global retry configuration across receive endpoints
                    cfg.UseMessageRetry(r =>
                    {
                        r.Incremental(
                            rabbitSettings.Retry.MaxRetryAttempts,
                            TimeSpan.FromSeconds(rabbitSettings.Retry.InitialIntervalSeconds),
                            TimeSpan.FromSeconds(rabbitSettings.Retry.IntervalIncrementSeconds));
                    });

                    // Dedicated endpoint for VacationApprovedConsumer with DLX / DLQ
                    cfg.ReceiveEndpoint(rabbitSettings.Retry.VacationApprovedQueue, e =>
                    {
                        e.SetQueueArgument("x-dead-letter-exchange", rabbitSettings.Retry.DeadLetterExchange);
                        e.SetQueueArgument("x-dead-letter-routing-key", rabbitSettings.Retry.DeadLetterQueue);

                        e.UseMessageRetry(r =>
                        {
                            r.Incremental(
                                rabbitSettings.Retry.MaxRetryAttempts,
                                TimeSpan.FromSeconds(rabbitSettings.Retry.InitialIntervalSeconds),
                                TimeSpan.FromSeconds(rabbitSettings.Retry.IntervalIncrementSeconds));
                        });

                        e.ConfigureConsumer<VacationApprovedConsumer>(context);
                    });

                    // Dedicated endpoint for VacationRejectedConsumer with DLX / DLQ
                    cfg.ReceiveEndpoint(rabbitSettings.Retry.VacationRejectedQueue, e =>
                    {
                        e.SetQueueArgument("x-dead-letter-exchange", rabbitSettings.Retry.DeadLetterExchange);
                        e.SetQueueArgument("x-dead-letter-routing-key", rabbitSettings.Retry.DeadLetterQueue);

                        e.UseMessageRetry(r =>
                        {
                            r.Incremental(
                                rabbitSettings.Retry.MaxRetryAttempts,
                                TimeSpan.FromSeconds(rabbitSettings.Retry.InitialIntervalSeconds),
                                TimeSpan.FromSeconds(rabbitSettings.Retry.IntervalIncrementSeconds));
                        });

                        e.ConfigureConsumer<VacationRejectedConsumer>(context);
                    });

                    // Dead Letter Queue endpoint ensuring DLX exchange and queue are provisioned
                    cfg.ReceiveEndpoint(rabbitSettings.Retry.DeadLetterQueue, e =>
                    {
                        e.Bind(rabbitSettings.Retry.DeadLetterExchange, b =>
                        {
                            b.RoutingKey = rabbitSettings.Retry.DeadLetterQueue;
                            b.ExchangeType = "direct";
                        });
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            // ── Repositories ──────────────────────────────────────────────────────
            services.AddScoped(typeof(IGeneralRepositoryInterface<>), typeof(GenericRepository<>));

            // Employee
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IGeneralRepositoryInterface<Employee>, EmployeeRepository>();

            // Organizational
            services.AddScoped<IGeneralRepositoryInterface<GeneralDepartment>, GeneralDepartmentRepository>();
            services.AddScoped<IGeneralRepositoryInterface<Department>, DepartmentRepository>();
            services.AddScoped<IGeneralRepositoryInterface<Branch>, BranchRepository>();

            // Location
            services.AddScoped<IGeneralRepositoryInterface<Country>, CountryRepository>();
            services.AddScoped<IGeneralRepositoryInterface<City>, CityRepository>();
            services.AddScoped<IGeneralRepositoryInterface<Town>, TownRepository>();

            // Vacation
            services.AddScoped<IVacationRepository, VacationRepository>();
            services.AddScoped<IGeneralRepositoryInterface<Vacation>, VacationRepository>();
            services.AddScoped<IGeneralRepositoryInterface<VacationType>, VacationTypeRepository>();

            // OverTime
            services.AddScoped<IOverTimeRepository, OverTimeRepository>();
            services.AddScoped<IGeneralRepositoryInterface<OverTime>, OverTimeRepository>();
            services.AddScoped<IGeneralRepositoryInterface<OverTimeType>, OverTimeTypeRepository>();

            // Doctor
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IGeneralRepositoryInterface<Doctor>, DoctorRepository>();

            // Sanction
            services.AddScoped<ISanctionRepository, SanctionRepository>();
            services.AddScoped<IGeneralRepositoryInterface<Sanction>, SanctionRepository>();
            services.AddScoped<IGeneralRepositoryInterface<SanctionType>, SanctionTypeRepository>();

            // Payroll
            services.AddScoped<IPayrollCalculator, PayrollCalculator>();
            services.AddScoped<IPayrollRepository, PayrollRepository>();
            services.AddScoped<IGeneralRepositoryInterface<Payroll>, PayrollRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            // Register Global Exception Handling Middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            // Register Global Exception Handling Middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }
    }
}
