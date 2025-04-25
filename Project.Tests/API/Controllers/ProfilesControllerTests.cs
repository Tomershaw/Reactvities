using API.Controllers;
using Application.Core;
using Application.Profiles;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.API.Controllers
{
    public class ProfilesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProfilesController _controller;

        public ProfilesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();

            _controller = new ProfilesController();
            typeof(ProfilesController)
                .BaseType
                .GetField("_mediator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(_controller, _mediatorMock.Object);
        }

        [Fact]
        public async Task GetProfile_Should_Return_Profile_When_User_Exists()
        {
            // Arrange
            var username = "testuser";
            var profile = new Profile
            {
                UserName = username,
                DisplayName = "Test User",
                Bio = "Test bio"
            };

            _mediatorMock.Setup(m => m.Send(It.Is<Details.Query>(q => q.Username == username), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Result<Profile>.Success(profile));

            // Act
            var result = await _controller.GetProfile(username);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProfile = Assert.IsType<Profile>(okResult.Value);
            Assert.Equal(username, returnedProfile.UserName);
        }

        [Fact]
public async Task GetProfile_Should_Return_BadRequest_When_Profile_Not_Found()
{
    // Arrange
    var username = "ghost";
    _mediatorMock.Setup(m => m.Send(
        It.IsAny<Details.Query>(),
        It.IsAny<CancellationToken>()))
        .ReturnsAsync(Result<Profile>.Failure("Profile not found"));

    // Act
    var result = await _controller.GetProfile(username);

    // Assert
    var badRequest = Assert.IsType<BadRequestObjectResult>(result);
    Assert.Equal("Profile not found", badRequest.Value);
}
    }
}
