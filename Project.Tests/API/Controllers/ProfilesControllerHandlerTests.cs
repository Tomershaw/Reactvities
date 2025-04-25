using Application.Core;
using Application.Interfaces;
using Application.Profiles;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.API.Handlers
{
    public class ProfilesControllerHandlerTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly Mock<IUserAccessor> _userAccessorMock;

        public ProfilesControllerHandlerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            var user = new Domain.AppUser
            {
                Id = "1",
                UserName = "testuser",
                DisplayName = "Test User",
                Bio = "This is a test bio"
            };

            _context.Users.Add(user);
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
        public async Task Handle_Should_Return_Profile_When_User_Exists()
        {
            var handler = new Details.Handler(_context, _mapper, _userAccessorMock.Object);

            var result = await handler.Handle(new Details.Query { Username = "testuser" }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal("testuser", result.Value.UserName);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_User_Not_Found()
        {
            var handler = new Details.Handler(_context, _mapper, _userAccessorMock.Object);

            var result = await handler.Handle(new Details.Query { Username = "nonexistent" }, CancellationToken.None);

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
