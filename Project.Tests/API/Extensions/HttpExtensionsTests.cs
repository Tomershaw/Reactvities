using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Xunit;
using API.Extensions;

namespace Project.Tests.API.Extensions
{
    public class HttpExtensionsTests
    {
        [Fact]
        public void AddPaginationHeader_Should_Add_Correct_Header()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var response = context.Response;

            int currentPage = 1;
            int itemsPerPage = 10;
            int totalItems = 50;
            int totalPages = 5;

            var expectedHeader = JsonSerializer.Serialize(new
            {
                currentPage,
                itemsPerPage,
                totalItems,
                totalPages
            });

            // Act
            response.AddPaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);

            // Assert
            Assert.True(response.Headers.ContainsKey("Pagination"));
            Assert.Equal(expectedHeader, response.Headers["Pagination"]);
        }
    }
}
