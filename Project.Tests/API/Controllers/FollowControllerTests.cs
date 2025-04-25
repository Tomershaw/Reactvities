using API.Controllers;
using Application.Followers;
using Application.Core;
using Application.Profiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.Tests.Utils; // ✅ מחייב את קובץ ההרחבות
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Project.Tests.API.Controllers
{
    public class FollowControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly FollowController _controller;

        public FollowControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new FollowController();
            _controller.SetMediator(_mediatorMock.Object); // 🧠 זה מגיע מה- TestExtensions.cs
        }

        [Fact]
public async Task Follow_Should_Return_Ok_When_Success()
{
    // Arrange
    var username = "bob";
    _mediatorMock.Setup(x =>
        x.Send(It.IsAny<FollowToggle.Command>(), It.IsAny<CancellationToken>())
    ).ReturnsAsync(Result<Unit>.Success(Unit.Value));

    // Act
    var result = await _controller.Follow(username);

    // Assert
    var ok = Assert.IsType<OkObjectResult>(result);
    Assert.NotNull(ok); // אפשר גם לבדוק את הערך אם אתה רוצה
}

        [Fact]
        public async Task GetFollowings_Should_Return_List_When_Success()
        {
            // Arrange
            var username = "bob";
            var predicate = "followers";
            var profileList = new List<Profile> { new Profile { UserName = "alice" } };

            _mediatorMock.Setup(x =>
                x.Send(It.Is<List.Query>(q => q.Username == username && q.Predicate == predicate), It.IsAny<CancellationToken>())
            ).ReturnsAsync(Result<List<Profile>>.Success(profileList));

            // Act
            var result = await _controller.GetFollowings(username, predicate);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<List<Profile>>(ok.Value);
            Assert.Single(list);
            Assert.Equal("alice", list[0].UserName);
        }
    }
}
