using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using Xunit;
using System.Collections.Generic;

namespace Project.Tests.Application.Activities
{
    public class UpdateAttendanceHandlerTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly Mock<IUserAccessor> _userAccessorMock;

        public UpdateAttendanceHandlerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            // ניצור פעילות ללא משתתפים
            var activity = new Activity
            {
                Id = Guid.NewGuid(),
                Title = "Test Activity",
                Date = DateTime.UtcNow.AddDays(1),
                Attendees = new List<ActivityAttendee>()
            };

            // ניצור משתמש שיתווסף כמשתתף
            var user = new AppUser
            {
                Id = "1",
                UserName = "testuser"
            };

            _context.Activities.Add(activity);
            _context.Users.Add(user);
            _context.SaveChanges();

            _userAccessorMock = new Mock<IUserAccessor>();
            _userAccessorMock.Setup(x => x.GetUsername()).Returns("testuser");
        }

        [Fact]
        public async Task Handle_Should_Add_User_As_Attendee_When_Not_Already_Joined()
        {
            // Arrange
            var activity = await _context.Activities.FirstAsync();
            var handler = new UpdateAttendance.Handler(_context, _userAccessorMock.Object);
            var command = new UpdateAttendance.Commend { Id = activity.Id };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var updatedActivity = await _context.Activities
                .Include(a => a.Attendees)
                .ThenInclude(x => x.AppUser)
                .FirstAsync(x => x.Id == activity.Id);

            var attendee = updatedActivity.Attendees.FirstOrDefault(a => a.AppUser.UserName == "testuser");

            Assert.NotNull(attendee);
            Assert.False(attendee.IsHost);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


[Fact]
public async Task Handle_Should_Remove_User_From_Attendees_When_Already_Attending_And_Not_Host()
{
    // Arrange: Add user ו-activity
    var user = new AppUser { Id = "user123", UserName = "testuser" };
    await _context.Users.AddAsync(user);
    await _context.SaveChangesAsync();

    var activity = new Activity
    {
        Id = Guid.NewGuid(),
        Title = "Test Activity",
        Date = DateTime.UtcNow.AddDays(1),
        Attendees = new List<ActivityAttendee>()
    };

    // שלוף את ה-user מתוך ה-DbContext כדי להימנע מבעיה של tracking כפול
    var trackedUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == "testuser");

    // הוסף את המשתמש לפעילות כ-attendee (לא host)
    activity.Attendees.Add(new ActivityAttendee
    {
        AppUser = trackedUser,
        IsHost = false
    });

    await _context.Activities.AddAsync(activity);
    await _context.SaveChangesAsync();

    // Setup mock
    var userAccessorMock = new Mock<IUserAccessor>();
    userAccessorMock.Setup(x => x.GetUsername()).Returns("testuser");

    var handler = new UpdateAttendance.Handler(_context, userAccessorMock.Object);

    // Act
    var result = await handler.Handle(new UpdateAttendance.Commend { Id = activity.Id }, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    var updatedActivity = await _context.Activities
        .Include(x => x.Attendees)
        .ThenInclude(a => a.AppUser)
        .FirstOrDefaultAsync(x => x.Id == activity.Id);

    Assert.NotNull(updatedActivity);
    Assert.Empty(updatedActivity.Attendees); // המשתמש הוסר
}


[Fact]
public async Task Handle_Should_Add_User_As_Attendee_When_Not_Already_Attending()
{
    // Arrange
    var activity = new Activity
    {
        Id = Guid.NewGuid(),
        Title = "Yoga Class",
        Date = DateTime.UtcNow.AddDays(1),
        Attendees = new List<ActivityAttendee>()
    };

    var user = new AppUser { Id = "99", UserName = "newuser" };

    _context.Activities.Add(activity);
    _context.Users.Add(user);
    _context.SaveChanges();

    _userAccessorMock.Setup(x => x.GetUsername()).Returns("newuser");

    var handler = new UpdateAttendance.Handler(_context, _userAccessorMock.Object);
    var command = new UpdateAttendance.Commend { Id = activity.Id };

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    var updatedActivity = await _context.Activities
        .Include(a => a.Attendees)
        .ThenInclude(a => a.AppUser)
        .FirstOrDefaultAsync(a => a.Id == activity.Id);

    Assert.Single(updatedActivity.Attendees);
    Assert.Equal("newuser", updatedActivity.Attendees.First().AppUser.UserName);
    Assert.False(updatedActivity.Attendees.First().IsHost);
}

[Fact]
public async Task Handle_Should_Toggle_IsCancelled_When_User_Is_Host()
{
    // Arrange
    var host = new AppUser { Id = "5", UserName = "hostuser" };
    await _context.Users.AddAsync(host);
    await _context.SaveChangesAsync();

    var activity = new Activity
    {
        Id = Guid.NewGuid(),
        Title = "Hosted Activity",
        Date = DateTime.UtcNow.AddDays(1),
        IsCancelled = false,
        Attendees = new List<ActivityAttendee>()
    };

    var trackedHost = await _context.Users.FirstOrDefaultAsync(x => x.UserName == "hostuser");

    activity.Attendees.Add(new ActivityAttendee
    {
        AppUser = trackedHost,
        IsHost = true
    });

    await _context.Activities.AddAsync(activity);
    await _context.SaveChangesAsync();

    _userAccessorMock.Setup(x => x.GetUsername()).Returns("hostuser");
    var handler = new UpdateAttendance.Handler(_context, _userAccessorMock.Object);

    // Act
    var result = await handler.Handle(new UpdateAttendance.Commend { Id = activity.Id }, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);

    var updatedActivity = await _context.Activities.FindAsync(activity.Id);
    Assert.True(updatedActivity.IsCancelled); // בודק שהסטטוס התהפך
}



    }
}
