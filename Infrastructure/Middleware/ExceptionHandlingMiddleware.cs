using Application.Service.Contracts.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (DbUpdateException ex)
            {
                var logger = context.RequestServices.GetService<IAppLogger<ExceptionHandlingMiddleware>>();

                context.Response.ContentType = "application/json";

                if (ex.InnerException is SqlException innerException)
                {
                    logger.LogError(innerException, "A database error occurred.");
                    switch (innerException.Number)
                    {
                        case 2627:
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = "Unique constraint violation"
                            }));
                            break;

                        case 515:
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = "Cannot insert null value"
                            }));
                            break;

                        case 547:
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = "An error occurred while processing your request (foreign key violation)"
                            }));
                            break;

                        default:
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = "An unexpected database error occurred."
                            }));
                            break;
                    }
                }
                else
                {
                    logger.LogError(ex, "Related Efcore Exception");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = "An unexpected error occurred."
                    }));
                }

            }
            catch (Exception ex)
            { 
              var logger = context.RequestServices.GetRequiredService<IAppLogger<ExceptionHandlingMiddleware>>();
            logger.LogError(ex, "Unknown Exception");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = $"An error occurred: {ex.Message}"
            }));
            }
        }
    }
}
       