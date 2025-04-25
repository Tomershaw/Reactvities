using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Comments;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using Xunit;

namespace Project.Tests.Application.Comments
{
    public class CreateCommentHandlerTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly Mock<IUserAccessor> _userAccessorMock;
        private IMapper _mapper;

        public CreateCommentHandlerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            // יצירת משתמש ופעילות
            var user = new AppUser
            {
                Id = "1",
                UserName = "commenter",
                Photos = new List<Photo>
                {
                    new Photo { Id = "photo1", Url = "http://photo.com/1", IsMain = true }
                }
            };

            var activity = new Activity
            {
                Id = Guid.NewGuid(),
                Title = "Sample Activity",
                Comments = new List<Comment>()
            };

            _context.Users.Add(user);
            _context.Activities.Add(activity);
            _context.SaveChanges();

            _userAccessorMock = new Mock<IUserAccessor>();
            _userAccessorMock.Setup(x => x.GetUsername()).Returns("commenter");
             
            var config = new MapperConfiguration(cfg =>
            {
             cfg.AddProfile(new MappingProfiles());
            });

                _mapper = config.CreateMapper();

        }
         
        [Fact]
        public async Task Handle_Should_Add_Comment_When_User_And_Activity_Exist()
        {
            // Arrange
            var activity = await _context.Activities.FirstAsync();
           var handler = new Create.Handler(_context, _mapper, _userAccessorMock.Object);


            var command = new Create.Command
            {
                ActivityId = activity.Id,
                Body = "This is a test comment"
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var updatedActivity = await _context.Activities
                .Include(a => a.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(a => a.Id == activity.Id);

            Assert.Single(updatedActivity.Comments);
            var comment = updatedActivity.Comments.First();
            Assert.Equal("This is a test comment", comment.Body);
            Assert.Equal("commenter", comment.Author.UserName);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
public async Task Handle_Should_Return_Failure_When_Activity_Not_Found()
{
    // Arrange
    var command = new Create.Command
    {
        Body = "This should fail",
        ActivityId = Guid.NewGuid() // מזהה שלא קיים
    };

    var handler = new Create.Handler(_context, _mapper, _userAccessorMock.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.False(result.IsSuccess);
    Assert.NotNull(result);
    Assert.Null(result.Value); // או שתבדוק תיאור כשל אם יש לך ב־Result
}

    }
}
