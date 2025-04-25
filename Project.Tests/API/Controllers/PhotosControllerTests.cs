using API.Controllers;
using Application.Photos;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.API.Controllers
{
    public class PhotosControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PhotosController _controller;

        public PhotosControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();

            _controller = new PhotosController();
            typeof(PhotosController)
                .BaseType
                .GetField("_mediator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(_controller, _mediatorMock.Object);
        }

        [Fact]
        public async Task Add_Should_Return_Ok_When_Success()
        {
            // Arrange
            var photo = new Photo { Id = "1", Url = "http://test.com/photo.jpg" };
            var command = new Add.Command();

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Result<Photo>.Success(photo));

            // Act
            var result = await _controller.Add(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPhoto = Assert.IsType<Photo>(okResult.Value);
            Assert.Equal(photo.Url, returnedPhoto.Url);
        }

        [Fact]
        public async Task Add_Should_Return_BadRequest_When_Failed()
        {
            // Arrange
            var command = new Add.Command();
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Result<Photo>.Failure("Add failed"));

            // Act
            var result = await _controller.Add(command);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Add failed", badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_Should_Return_Ok_When_Success()
        {
            // Arrange
            var photoId = "abc123";
            _mediatorMock.Setup(m => m.Send(It.IsAny<Delete.Commend>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Result<Unit>.Success(Unit.Value));

            // Act
            var result = await _controller.Delete(photoId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Delete_Should_Return_BadRequest_When_Failed()
        {
            // Arrange
            var photoId = "abc123";
            _mediatorMock.Setup(m => m.Send(It.IsAny<Delete.Commend>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Result<Unit>.Failure("Delete failed"));

            // Act
            var result = await _controller.Delete(photoId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Delete failed", badRequestResult.Value);
        }

        [Fact]
        public async Task SetMain_Should_Return_Ok_When_Success()
        {
            // Arrange
            var photoId = "abc123";
            _mediatorMock.Setup(m => m.Send(It.IsAny<SetMain.Commend>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Result<Unit>.Success(Unit.Value));

            // Act
            var result = await _controller.SetMain(photoId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task SetMain_Should_Return_BadRequest_When_Failed()
        {
            // Arrange
            var photoId = "abc123";
            _mediatorMock.Setup(m => m.Send(It.IsAny<SetMain.Commend>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Result<Unit>.Failure("SetMain failed"));

            // Act
            var result = await _controller.SetMain(photoId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("SetMain failed", badRequestResult.Value);
        }
    }
}
