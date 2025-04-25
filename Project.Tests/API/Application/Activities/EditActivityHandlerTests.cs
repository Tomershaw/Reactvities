using Application.Activities;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.Application.Activities
{
    public class EditActivityHandlerTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly Mock<IUserAccessor> _userAccessorMock;

        public EditActivityHandlerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            var activity = new Activity
            {
                Id = Guid.NewGuid(),
                Title = "Original Title",
                Description = "Original Description",
                Category = "music"
            };

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
        public async Task Handle_Should_Update_Activity_When_Exists()
        {
            // Arrange
            var existingActivity = await _context.Activities.FirstAsync();
            var updatedActivity = new Activity
            {
                Id = existingActivity.Id,
                Title = "Updated Title",
                Description = "Updated Description",
                Category = "sports"
            };

            var command = new Edit.Command { Activity = updatedActivity };
            var handler = new Edit.Handler(_context, _mapper);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var modified = await _context.Activities.FindAsync(existingActivity.Id);
            Assert.NotNull(modified); // Ensure modified is not null before further assertions
            Assert.Equal("Updated Title", modified!.Title); // Using null-forgiving operator
            Assert.Equal("Updated Description", modified!.Description); // Using null-forgiving operator
            Assert.Equal("sports", modified!.Category); // Using null-forgiving operator
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Activity_Does_Not_Exist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            var command = new Edit.Command
            {
                Activity = new Activity
                {
                    Id = nonExistentId,
                    Title = "Doesn't Matter"
                }
            };

            var handler = new Edit.Handler(_context, _mapper);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Activity not found", result.Error);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
