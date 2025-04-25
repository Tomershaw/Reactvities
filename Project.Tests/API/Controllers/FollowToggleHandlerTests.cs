using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Followers;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using Xunit;

namespace Project.Tests.Application.Followers
{
    public class FollowToggleHandlerTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly Mock<IUserAccessor> _userAccessorMock;

        public FollowToggleHandlerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            // Add dummy users
            var observer = new AppUser { Id = "1", UserName = "observer" };
            var target = new AppUser { Id = "2", UserName = "target" };

            _context.Users.Add(observer);
            _context.Users.Add(target);
            _context.SaveChanges();

            _userAccessorMock = new Mock<IUserAccessor>();
            _userAccessorMock.Setup(x => x.GetUsername()).Returns("observer");
        }

        [Fact]
        public async Task Handle_Should_Add_Follow_When_Not_Already_Following()
        {
            // Arrange
            var handler = new FollowToggle.Handler(_context, _userAccessorMock.Object);

            // Act
            var result = await handler.Handle(
                new FollowToggle.Command { TargetUsername = "target" },
                CancellationToken.None
            );

            // Assert
            Assert.True(result.IsSuccess);
            var follow = await _context.UserFollowings.FindAsync("1", "2");
            Assert.NotNull(follow);
        }

        [Fact]
        public async Task Handle_Should_Remove_Follow_When_Already_Following()
        {
            // Arrange
            _context.UserFollowings.Add(new UserFollowing
            {
                ObserverId = "1",
                TargetId = "2"
            });
            _context.SaveChanges();

            var handler = new FollowToggle.Handler(_context, _userAccessorMock.Object);

            // Act
            var result = await handler.Handle(
                new FollowToggle.Command { TargetUsername = "target" },
                CancellationToken.None
            );

            // Assert
            Assert.True(result.IsSuccess);
            var follow = await _context.UserFollowings.FindAsync("1", "2");
            Assert.Null(follow);
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_Target_Not_Found()
        {
            // Arrange
            var handler = new FollowToggle.Handler(_context, _userAccessorMock.Object);

            // Act
            var result = await handler.Handle(
                new FollowToggle.Command { TargetUsername = "notexists" },
                CancellationToken.None
            );

            // Assert
            Assert.Null(result);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
