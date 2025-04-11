using System.Net;
using System.Text.Json;
using Application.Core;
using Microsoft.AspNetCore.Http;

namespace API.MiddleWare
{
    // Custom middleware for centralized exception handling
    // Intercepts unhandled exceptions and returns a consistent error response

    public class ExceptionMiddleware
    {
        // Logger for recording exception details
        public readonly ILogger<ExceptionMiddleware> _logger;

        // The next delegate in the request pipeline
        public readonly RequestDelegate _next;

        // Hosting environment (used to show detailed errors in development only)
        private readonly IHostEnvironment _env;

        // Constructor injecting the request pipeline delegate, logger, and environment
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _next = next;
            _logger = logger;
        }

        // Middleware entry point: wraps the rest of the pipeline with a try-catch
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pass control to the next middleware/component in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the error details
                _logger.LogError(ex, ex.Message);

                // Set response type and status code
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Construct standardized error response based on environment
                var response = _env.IsDevelopment()
                    ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new AppException(context.Response.StatusCode, "Internal Server Error");

                // Configure JSON output formatting
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                // Serialize the error response and write to response body
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
