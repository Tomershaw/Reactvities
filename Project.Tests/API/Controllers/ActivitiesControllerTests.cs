using Application.Activities;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.Tests.API.Controllers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Project.Tests.API.Controllers;

namespace Project.Tests.API.Controllers
{
    public class ActivitiesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TestableActivitiesController _controller;

        public ActivitiesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();

            _controller = new TestableActivitiesController();
            _controller.SetMediator(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetActivity_Should_Return_Ok_When_Activity_Found()
        {
            // Arrange
            var activityId = Guid.NewGuid();
            var activityDto = new ActivityDto { Id = activityId, Title = "Test Activity" };

            _mediatorMock.Setup(m => m.Send(
                It.Is<Detalis.Query>(q => q.Id == activityId),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ActivityDto>.Success(activityDto));

            // Act
            var result = await _controller.GetActivity(activityId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedActivity = Assert.IsType<ActivityDto>(okResult.Value);
            Assert.Equal(activityId, returnedActivity.Id);
        }

[Fact]
public async Task GetActivity_Should_Return_BadRequest_When_Activity_Not_Found()
{
    // Arrange
    var activityId = Guid.NewGuid();

    _mediatorMock.Setup(m => m.Send(
        It.Is<Detalis.Query>(q => q.Id == activityId),
        It.IsAny<CancellationToken>()))
        .ReturnsAsync(Result<ActivityDto>.Failure("Activity not found"));

    // Act
    var result = await _controller.GetActivity(activityId);

    // Assert
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    var errorMessage = Assert.IsType<string>(badRequestResult.Value);
    Assert.Equal("Activity not found", errorMessage);
}


        [Fact]
        public async Task GetActivity_Should_Return_InternalServerError_When_Exception_Occurs()
        {
            // Arrange
            var activityId = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(
                It.Is<Detalis.Query>(q => q.Id == activityId),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetActivity(activityId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            var errorMessage = Assert.IsType<string>(statusCodeResult.Value);
            Assert.Equal("Unexpected error", errorMessage);
        }
    }
}
