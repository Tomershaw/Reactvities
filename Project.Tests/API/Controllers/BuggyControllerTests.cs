using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace Project.Tests.API.Controllers
{
    public class BuggyControllerTests
    {
        private readonly BuggyController _controller;

        public BuggyControllerTests()
        {
            _controller = new BuggyController();
        }

        [Fact]
        public void GetNotFound_Should_Return_404()
        {
            // Act
            var result = _controller.GetNotFound();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetBadRequest_Should_Return_400_With_Message()
        {
            // Act
            var result = _controller.GetBadRequest();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("This is a bad request", badRequestResult.Value);
        }

        [Fact]
        public void GetUnauthorised_Should_Return_401()
        {
            // Act
            var result = _controller.GetUnauthorised();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void GetServerError_Should_Throw_Exception()
        {
            // Act + Assert
            var ex = Assert.Throws<Exception>(() => _controller.GetServerError());
            Assert.Equal("This is a server error", ex.Message);
        }
    }
}
