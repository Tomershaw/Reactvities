using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using Xunit;

namespace Project.Tests.API.Application.Activities
{
    public class CreateActivityHandlerTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly Mock<IUserAccessor> _userAccessorMock;

        public CreateActivityHandlerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            // הוספת משתמש מזויף כדי לדמות סביבה אמיתית
            var user = new AppUser
            {
                Id = "1",
                UserName = "testuser",
                Email = "test@example.com"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            _userAccessorMock = new Mock<IUserAccessor>();
            _userAccessorMock.Setup(x => x.GetUsername()).Returns("testuser");
        }

        [Fact]
        public async Task Handle_Should_Create_Activity_And_Assign_Host()
        {
            // Arrange
            var activity = new Activity
            {
                Id = Guid.NewGuid(),
                Title = "Test Activity",
                Description = "Test Description",
                Date = DateTime.UtcNow.AddDays(1),
                Category = "Test",
                City = "TestCity",
                Venue = "TestVenue",
                Attendees = new List<ActivityAttendee>()
            };

            var command = new Create.Commend { Activity = activity };

            var handler = new Create.Handler(_context, _userAccessorMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var createdActivity = await _context.Activities
                .Include(a => a.Attendees)
                .ThenInclude(aa => aa.AppUser)
                .FirstOrDefaultAsync(a => a.Id == activity.Id);

            Assert.NotNull(createdActivity);
            Assert.Single(createdActivity.Attendees);
            Assert.True(createdActivity.Attendees.First().IsHost);
            Assert.Equal("testuser", createdActivity.Attendees.First().AppUser.UserName);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
