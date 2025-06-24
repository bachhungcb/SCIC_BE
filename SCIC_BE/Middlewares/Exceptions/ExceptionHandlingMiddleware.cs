using System;
using System.Collections.Generic;
using SCIC_BE.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SCIC_BE.Middlewares.Exceptions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate requestDelegate,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = requestDelegate;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // tiếp tục pipeline
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode = ex switch
            {
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ArgumentException or InvalidOperationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            string errorCode = ex switch
            {
                UnauthorizedAccessException => "unauthorized",
                KeyNotFoundException => "not_found",
                ArgumentException or InvalidOperationException => "bad_request",
                _ => "internal_error"
            };

            var traceId = context.TraceIdentifier;

            _logger.LogError(ex, "[{TraceId}] Unhandled Exception: {Message}", traceId, ex.Message);

            var response = new ApiError
            {
                Status = statusCode,
                Message = ex.Message,
                ErrorCode = errorCode,
                TraceId = traceId,
                Timestamp = DateTime.UtcNow.ToString("o"),
                Details = _env.IsDevelopment() ? ex.ToString() : null
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }

}
