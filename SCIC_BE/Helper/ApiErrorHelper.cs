using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SCIC_BE.Models;

namespace SCIC_BE.Helper
{
    public static class ApiErrorHelper
    {
        public static ApiErrorResult Build(int status, string message, HttpContext httpContext)
        {
            return new ApiErrorResult
            {
                Status = status,
                Message = message,
                TraceId = httpContext.TraceIdentifier,
                Timestamp = DateTime.UtcNow.ToString("o")
            };
        }
    }
}
