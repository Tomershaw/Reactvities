using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using API.MiddleWare;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting; // חשוב!
using Moq;
using Xunit;

namespace Project.Tests.API.Middleware
{
    public class ExceptionMiddlewareUnitTests
    {
        [Fact]
        public async Task Invoke_Should_Return_Json_Error_Response_When_Exception_Thrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            var loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
            var envMock = new Mock<IHostEnvironment>();
            envMock.Setup(e => e.EnvironmentName).Returns("Development"); // או "Production" אם אתה רוצה לבדוק בלי פירוט

            var middleware = new ExceptionMiddleware(
                async (innerHttpContext) =>
                {
                    throw new Exception("Something went wrong!");
                },
                loggerMock.Object,
                envMock.Object
            );

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            responseStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(responseStream);
            var responseText = await reader.ReadToEndAsync();

            Assert.Equal(500, context.Response.StatusCode);
            Assert.Contains("Something went wrong!", responseText);

            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(500, errorResponse.StatusCode);
        }

        [Fact]
        public async Task Invoke_Should_Hide_Error_Details_In_Production()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            var loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
            var envMock = new Mock<IHostEnvironment>();
            envMock.Setup(e => e.EnvironmentName).Returns("Production"); // Simulate production environment

            var middleware = new ExceptionMiddleware(
                async (innerHttpContext) =>
                {
                    throw new Exception("Sensitive error details!");
                },
                loggerMock.Object,
                envMock.Object
            );

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            responseStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(responseStream);
            var responseText = await reader.ReadToEndAsync();

            Assert.Equal(500, context.Response.StatusCode);
            Assert.DoesNotContain("Sensitive error details!", responseText); // Ensure sensitive details are not exposed

            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(500, errorResponse.StatusCode);
            Assert.Equal("Internal Server Error", errorResponse.Message); // Generic error message
            Assert.Null(errorResponse.Details); // No detailed error information in production
        }

        private class ErrorResponse
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public string Details { get; set; }
        }
    }
}
