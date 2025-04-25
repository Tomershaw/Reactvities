using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Comments;
using Application.Core;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Xunit;

namespace Project.Tests.Application.Comments
{
    public class ListCommentsHandlerTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ListCommentsHandlerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_Should_Return_Comments_For_Specified_Activity_Ordered_By_CreationDate()
        {
            // Arrange
            var activityId = Guid.NewGuid();

            var activity = new Activity
            {
                Id = activityId,
                Title = "Activity with Comments"
            };

            var user = new AppUser
            {
                Id = "1",
                UserName = "testuser"
            };

            var comments = new List<Comment>
            {
                new Comment { Body = "First comment", CreateAt = DateTime.UtcNow.AddMinutes(-10), Author = user, Activity = activity },
                new Comment { Body = "Second comment", CreateAt = DateTime.UtcNow, Author = user, Activity = activity }
            };

            _context.Users.Add(user);
            _context.Activities.Add(activity);
            _context.Comments.AddRange(comments);
            await _context.SaveChangesAsync();

            var handler = new List.Handler(_context, _mapper);
            var query = new List.Query { ActivityId = activityId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);
            Assert.Equal("Second comment", result.Value[0].Body); // Latest comment first
            Assert.Equal("First comment", result.Value[1].Body);
        }

        [Fact]
public async Task Handle_Should_Return_Empty_List_When_Activity_Has_No_Comments()
{
    // Arrange
    var nonExistingActivityId = Guid.NewGuid(); // No comments added

    var handler = new List.Handler(_context, _mapper);
    var query = new List.Query { ActivityId = nonExistingActivityId };

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Empty(result.Value);
    Assert.NotNull(result.Value);
    Assert.Empty(result.Value); // Ensure it's an empty list
}
 [Fact]
public async Task Handle_Should_Return_Comments_For_Activity_Sorted_By_CreateAt_Descending()
{
    // Arrange
    var activityId = Guid.NewGuid();

    var activity = new Activity
    {
        Id = activityId,
        Title = "Activity with comments"
    };

    var comments = new List<Comment>
    {
        new Comment
        {
            Body = "First comment",
            CreateAt = DateTime.UtcNow.AddMinutes(-10),
            Activity = activity
        },
        new Comment
        {
            Body = "Second comment",
            CreateAt = DateTime.UtcNow.AddMinutes(-5),
            Activity = activity
        }
    };

    _context.Activities.Add(activity);
    _context.Comments.AddRange(comments);
    await _context.SaveChangesAsync();

    var handler = new List.Handler(_context, _mapper);
    var query = new List.Query { ActivityId = activityId };

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(2, result.Value.Count);
    Assert.True(result.Value[0].CreateAt >= result.Value[1].CreateAt);
}


        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}

