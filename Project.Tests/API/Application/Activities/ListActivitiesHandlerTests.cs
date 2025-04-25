using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using Xunit;

namespace Project.Tests.Application.Activities
{
    public class ListHandlerTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly Mock<IUserAccessor> _userAccessorMock;
        private readonly IMapper _mapper;

        public ListHandlerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _context = new DataContext(options);

            // יוצרים משתמש ודוחפים אותו לפעילות כמארח
            var user = new AppUser { Id = "1", UserName = "hostuser" };
            var activity = new Activity
            {
                Id = Guid.NewGuid(),
                Title = "Test Activity",
                Date = DateTime.UtcNow.AddDays(1),
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
            _userAccessorMock.Setup(x => x.GetUsername()).Returns("hostuser");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_Should_Return_Only_Hosted_Activities()
        {
            // Arrange
            var handler = new List.Handler(_context, _mapper, _userAccessorMock.Object);
            var request = new List.Query
            {
                Params = new ActivityParams
                {
                    IsHost = true,
                    IsGoing = false,
                    StartDate = DateTime.UtcNow,
                    PageNumber = 1,
                    PageSize = 10
                }
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value); // רק פעילות אחת שהיוזר מארח
            Assert.Equal("Test Activity", result.Value[0].Title);
            Assert.Equal("hostuser", result.Value[0].HostUsername);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        [Fact]
public async Task Handle_Should_Return_Only_Activities_User_Is_Going()
{
    // Arrange
    var user = new AppUser { Id = "2", UserName = "attendinguser" };
    var activity = new Activity
    {
        Id = Guid.NewGuid(),
        Title = "Attending Activity",
        Date = DateTime.UtcNow.AddDays(2),
        Attendees = new List<ActivityAttendee>
        {
            new ActivityAttendee
            {
                AppUser = user,
                IsHost = false
            }
        }
    };

    _context.Users.Add(user);
    _context.Activities.Add(activity);
    _context.SaveChanges();

    _userAccessorMock.Setup(x => x.GetUsername()).Returns("attendinguser");

    var handler = new List.Handler(_context, _mapper, _userAccessorMock.Object);
    var request = new List.Query
    {
        Params = new ActivityParams
        {
            IsGoing = true,
            IsHost = false,
            StartDate = DateTime.UtcNow,
            PageNumber = 1,
            PageSize = 10
        }
    };

    // Act
    var result = await handler.Handle(request, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Single(result.Value);
    Assert.Equal("Attending Activity", result.Value[0].Title);
}

[Fact]
public async Task Handle_Should_Return_Only_Activities_User_Is_Hosting()
{
    // Arrange
    var user = new AppUser { Id = "3", UserName = "hostonlyuser" };
    var activity = new Activity
    {
        Title = "Hosted Activity",
        Date = DateTime.UtcNow.AddDays(3),
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

    _userAccessorMock.Setup(x => x.GetUsername()).Returns("hostonlyuser");

    var handler = new List.Handler(_context, _mapper, _userAccessorMock.Object);

    var request = new List.Query
    {
        Params = new ActivityParams
        {
            IsHost = true,
            IsGoing = false,
            StartDate = DateTime.UtcNow,
            PageNumber = 1,
            PageSize = 10
        }
    };

    // Act
    var result = await handler.Handle(request, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Single(result.Value);
    Assert.Equal("Hosted Activity", result.Value[0].Title);
    Assert.Equal("hostonlyuser", result.Value[0].HostUsername);
}

[Fact]
public async Task Handle_Should_Return_All_Upcoming_Activities_When_No_Filters_Applied()
{
    // ניקוי פעילות קודמות (כי ה־DB משותף לכל הטסטים בקובץ)
    _context.Activities.RemoveRange(_context.Activities);
    _context.SaveChanges();

    // Arrange
    var user = new AppUser { Id = "4", UserName = "nonparticipant" };

    var activity1 = new Activity
    {
        Title = "General Activity 1",
        Date = DateTime.UtcNow.AddDays(2),
    };

    var activity2 = new Activity
    {
        Title = "General Activity 2",
        Date = DateTime.UtcNow.AddDays(3),
    };

    _context.Users.Add(user);
    _context.Activities.AddRange(activity1, activity2);
    _context.SaveChanges();

    _userAccessorMock.Setup(x => x.GetUsername()).Returns("nonparticipant");

    var handler = new List.Handler(_context, _mapper, _userAccessorMock.Object);

    var request = new List.Query
    {
        Params = new ActivityParams
        {
            IsHost = false,
            IsGoing = false,
            StartDate = DateTime.UtcNow,
            PageNumber = 1,
            PageSize = 10
        }
    };

    // Act
    var result = await handler.Handle(request, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(2, result.Value.Count);
    Assert.Contains(result.Value, x => x.Title == "General Activity 1");
    Assert.Contains(result.Value, x => x.Title == "General Activity 2");
}


    }
}
