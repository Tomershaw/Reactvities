using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using Xunit;

namespace Project.Tests.API.Application.Activities
{
    public class CreateDeleteHandlerTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly Mock<IUserAccessor> _userAccessorMock;

        public CreateDeleteHandlerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            // הוספת משתמש לבדיקה
            var user = new AppUser { Id = "1", UserName = "testuser", Email = "test@example.com" };
            _context.Users.Add(user);
            _context.SaveChanges();

            _userAccessorMock = new Mock<IUserAccessor>();
            _userAccessorMock.Setup(x => x.GetUsername()).Returns("testuser");
        }

        [Fact]
        public async Task CreateActivity_Should_Add_Activity_With_Host()
        {
            // Arrange
            var activity = new Activity
            {
                Id = Guid.NewGuid(),
                Title = "Test Activity",
                Description = "Description",
                Date = DateTime.UtcNow.AddDays(1),
                City = "Tel Aviv",
                Venue = "Beach"
            };

            var command = new Create.Commend { Activity = activity };
            var handler = new Create.Handler(_context, _userAccessorMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var created = await _context.Activities.Include(a => a.Attendees).FirstOrDefaultAsync(a => a.Id == activity.Id);
            Assert.NotNull(created);
            Assert.Single(created.Attendees);
            Assert.Equal("testuser", created.Attendees.First().AppUser.UserName);
            Assert.True(created.Attendees.First().IsHost);
        }

        [Fact]
        public async Task DeleteActivity_Should_Remove_Activity_When_Found()
        {
            // Arrange
            var activity = new Activity
            {
                Id = Guid.NewGuid(),
                Title = "Activity To Delete",
                Description = "To be deleted",
                Date = DateTime.UtcNow.AddDays(2),
                City = "Jerusalem",
                Venue = "Center"
            };

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            var command = new Delete.Commend { Id = activity.Id };
            var handler = new Delete.Handler(_context);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var deleted = await _context.Activities.FindAsync(activity.Id);
            Assert.Null(deleted);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
