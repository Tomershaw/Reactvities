using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using MockQueryable.Moq;
using Microsoft.AspNetCore.Http; // ⬅️ מוסיף תמיכה ב- IQueryable עם EF async

namespace Project.Tests.API.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly TokenService _tokenService;
        private readonly Mock<IConfiguration> _configMock;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(),
                null!, // PasswordHasher
                null!, // UserValidators
                null!, // PasswordValidators
                null!, // KeyNormalizer
                null!, // ErrorsDescriber
                null!, // Services
                null!, // ContextAccessor
                null!  // Logger
            );

            _configMock = new Mock<IConfiguration>();
            _configMock.Setup(x => x["TokenKey"]).Returns("supersecretkey_supersecretkey_supersecretkey_supersecretkey_123456");


            _tokenService = new TokenService(_configMock.Object);

            _controller = new AccountController(
                _userManagerMock.Object,
                _tokenService,
                _configMock.Object
            );
        }

        [Fact]
        public async Task Login_Should_Return_Unauthorized_When_User_Not_Found()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "notfound@example.com",
                Password = "wrongpass"
            };

              var users = new List<AppUser>().AsQueryable().BuildMockDbSet();
             _userManagerMock.Setup(x => x.Users).Returns(users.Object);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }
          
        [Fact]
        public async Task Login_Should_Return_UserDto_When_Credentials_Are_Correct()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "correctpassword"
            };

            var user = new AppUser
            {
                Email = "test@example.com",
                UserName = "testuser",
                DisplayName = "Test User",
                Id = "123",
                RefreshTokens = new List<RefreshToken>() // ✅ חשוב
            };

            var users = new List<AppUser> { user }
                .AsQueryable()
                .BuildMockDbSet();

            _userManagerMock.Setup(x => x.Users).Returns(users.Object);

            _userManagerMock.Setup(x =>
                x.CheckPasswordAsync(user, loginDto.Password)
            ).ReturnsAsync(true);

            _userManagerMock.Setup(x =>
                x.UpdateAsync(user)
            ).ReturnsAsync(IdentityResult.Success); // ✅ חשוב

            // ✅ מוסיף HttpContext כדי שלא יקרוס על Cookies
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext

            };

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<ActionResult<UserDto>>(result);
            var userDto = Assert.IsType<UserDto>(okResult.Value);

            Assert.Equal(user.UserName, userDto.Username);
            Assert.Equal(user.DisplayName, userDto.DisplayName);
            Assert.NotNull(userDto.Token);
        }

        [Fact]
        public async Task Login_Should_Return_Unauthorized_When_Password_Is_Incorrect()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var user = new AppUser
            {
                Email = "test@example.com",
                UserName = "testuser",
                DisplayName = "Test User",
                Id = "123",
                RefreshTokens = new List<RefreshToken>()
            };

            var users = new List<AppUser> { user }
                .AsQueryable()
                .BuildMockDbSet();

            _userManagerMock.Setup(x => x.Users).Returns(users.Object);

            _userManagerMock.Setup(x =>
                x.CheckPasswordAsync(user, loginDto.Password)
            ).ReturnsAsync(false);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

       [Fact]
public async Task Login_Should_Return_Forbidden_When_User_Is_Locked_Out()
{
    // Arrange
    var loginDto = new LoginDto
    {
        Email = "lockedout@example.com",
        Password = "correctpassword"
    };

    var user = new AppUser
    {
        Email = "lockedout@example.com",
        UserName = "lockeduser",
        DisplayName = "Locked User",
        Id = "456",
        LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(5), // Simulate locked-out state
        RefreshTokens = new List<RefreshToken>() // ✅ חובה כדי למנוע NullReference
    };

    var users = new List<AppUser> { user }
        .AsQueryable()
        .BuildMockDbSet();

    _userManagerMock.Setup(x => x.Users).Returns(users.Object);

    _userManagerMock.Setup(x =>
        x.CheckPasswordAsync(user, loginDto.Password)
    ).ReturnsAsync(true);

    _userManagerMock.Setup(x =>
        x.IsLockedOutAsync(user)
    ).ReturnsAsync(true);

    _userManagerMock.Setup(x =>
        x.UpdateAsync(user)
    ).ReturnsAsync(IdentityResult.Success); // ✅ חובה אם SetRefreshToken מתבצע

    // ✅ מוסיף HttpContext מזויף בשביל Response.Cookies
    var httpContext = new DefaultHttpContext();
    _controller.ControllerContext = new ControllerContext
    {
        HttpContext = httpContext
    };

    // Act
    var result = await _controller.Login(loginDto);

    // Assert
    Assert.IsType<ForbidResult>(result.Result);
}

    }
}
