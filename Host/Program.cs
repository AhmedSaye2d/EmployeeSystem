using Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logPath = Path.Combine(builder.Environment.ContentRootPath, "Logs", "log-.txt");

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day);
            });

            Log.Logger.Information("Application is building .......");

            // Add essential services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Custom dependency injections
            builder.Services.AddInfrastructureService(builder.Configuration);

            // Enable CORS for frontend
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            try
            {
                var app = builder.Build();

                app.UseCors();
                app.UseSerilogRequestLogging();

                app.UseInfrastructure();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseDefaultFiles();
                app.UseStaticFiles();

                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();

                Log.Logger.Information("Application is Running .......");

                app.Run();
            }
            catch (Exception ex) when (ex is not HostAbortedException)
            {
                Log.Logger.Error(ex, "Application failed to start......");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
