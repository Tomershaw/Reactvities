using System.Net;
using System.Text.Json;
using Application.Core;
using Microsoft.AspNetCore.Http;

namespace API.MiddleWare
{
    /// <summary>
    /// Middleware for centralized exception handling.
    /// Captures unhandled exceptions and returns a standardized error response.
    /// </summary>
    public class ExceptionMiddleware
    {
        /// <summary>
        /// Logger for recording exception details.
        /// </summary>
        public readonly ILogger<ExceptionMiddleware> _logger;

        /// <summary>
        /// The next delegate in the request pipeline.
        /// </summary>
        public readonly RequestDelegate _next;

        /// <summary>
        /// Hosting environment (used to show detailed errors in development only).
        /// </summary>
        private readonly IHostEnvironment _env;

        /// <summary>
        /// Constructor: Injects the request pipeline delegate, logger, and environment.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">Logger for recording exception details.</param>
        /// <param name="env">Hosting environment for determining error detail level.</param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Middleware entry point: wraps the rest of the pipeline with a try-catch block.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pass control to the next middleware/component in the pipeline.
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception details.
                _logger.LogError(ex, ex.Message);

                // Set the response content type and status code.
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Construct a standardized error response based on the environment.
                var response = _env.IsDevelopment()
                    ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new AppException(context.Response.StatusCode, "Internal Server Error");

                // Configure JSON output formatting for the error response.
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                // Serialize the error response and write it to the response body.
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
