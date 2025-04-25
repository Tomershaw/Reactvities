using System.Net;
using System.Threading.Tasks;
using API.MiddleWare;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Xunit;

namespace Project.Tests.API.MiddleWare
{
    public class ExceptionMiddlewareTests
    {
        [Fact]
        public async Task ExceptionMiddleware_Should_Return_500_And_Error_Details()
        {
            // Arrange
            var builder = new WebHostBuilder()
                .UseEnvironment("Development") // חובה כדי לחשוף את פרטי השגיאה!
                .ConfigureServices(services => { })
                .Configure(app =>
                {
                    // מוסיפים את המידלוור שמטפל בשגיאות
                    app.UseMiddleware<ExceptionMiddleware>();

                    // זורקים שגיאה שנרצה לבדוק שהמערכת מטפלת בה
                    app.Run(context => throw new System.Exception("Test exception"));
                });

            var server = new TestServer(builder);
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();

            // בודקים שהשגיאה שבכותרת אכן מופיעה בגוף התשובה
            Assert.Contains("Test exception", content);
        }

        [Fact]
        public async Task ExceptionMiddleware_Should_Not_Intercept_Successful_Request()
        {
            // Arrange
            var builder = new WebHostBuilder()
                .UseEnvironment("Development")
                .ConfigureServices(services => { })
                .Configure(app =>
                {
                    app.UseMiddleware<ExceptionMiddleware>();

                    app.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        await context.Response.WriteAsync("Success");
                    });
                });

            var server = new TestServer(builder);
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("Success", content);
        }
    }
}
