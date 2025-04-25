using Application.Activities;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.Application.Activities
{
    public class DetalisHandlerTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly Mock<IUserAccessor> _userAccessorMock;

        public DetalisHandlerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            var user = new AppUser
            {
                Id = "1",
                UserName = "testuser",
                Photos = new List<Photo>()
            };

            var activity = new Activity
            {
                Id = Guid.NewGuid(),
                Title = "Test Activity",
                Attendees = new List<ActivityAttendee>
                {
                    new ActivityAttendee
                    {
                        AppUser = user,
                        IsHost = true
                    }
                }
            };

            _context.Users.Add(user);
            _context.Activities.Add(activity);
            _context.SaveChanges();

            _userAccessorMock = new Mock<IUserAccessor>();
            _userAccessorMock.Setup(x => x.GetUsername()).Returns("testuser");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_Should_Return_ActivityDto_When_Activity_Exists()
        {
            // Arrange
            var activity = await _context.Activities.FirstAsync();
            var handler = new Detalis.Handler(_context, _mapper, _userAccessorMock.Object);

            // Act
            var result = await handler.Handle(new Detalis.Query { Id = activity.Id }, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(activity.Id, result.Value.Id);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Activity_Does_Not_Exist()
        {
            // Arrange
            var handler = new Detalis.Handler(_context, _mapper, _userAccessorMock.Object);
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await handler.Handle(new Detalis.Query { Id = nonExistentId }, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
