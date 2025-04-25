using Application.Photos;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.API.Handlers
{
    public class PhotosControllerHandlerTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly Mock<IUserAccessor> _userAccessorMock;
        private readonly Mock<IPhotoAccessor> _photoAccessorMock;

        public PhotosControllerHandlerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            var user = new AppUser
            {
                UserName = "testuser",
                Photos = new List<Photo>()
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            _userAccessorMock = new Mock<IUserAccessor>();
            _userAccessorMock.Setup(x => x.GetUsername()).Returns("testuser");

            _photoAccessorMock = new Mock<IPhotoAccessor>();
            _photoAccessorMock.Setup(x => x.AddPhoto(It.IsAny<IFormFile>()))
                .ReturnsAsync(new PhotoUploadResult
                {
                    Url = "http://test.com/photo.jpg",
                    PublicId = "abc123"
                });
        }

        [Fact]
        public async Task Handle_Should_Add_Photo_And_Return_Result()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            // var content = "fake content";
            // var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            // fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            // fileMock.Setup(f => f.FileName).Returns("photo.jpg");
            // fileMock.Setup(f => f.Length).Returns(stream.Length);

            var command = new Add.Command { File = fileMock.Object };
            var handler = new Add.Handler(_context, _photoAccessorMock.Object, _userAccessorMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("abc123", result.Value.Id);
            Assert.Equal("http://test.com/photo.jpg", result.Value.Url);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
