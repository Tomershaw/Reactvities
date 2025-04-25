using System;
using System.Threading;
using System.Threading.Tasks;
using API.SignalR;
using Application.Comments;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace Project.Tests.API.SignalR
{
    public class ChatHubHandlerTests
    {
        [Fact]
        public async Task SendComment_Should_Broadcast_To_Group_When_Successful()
        {
            // Arrange
            var activityId = Guid.NewGuid();
            var commentDto = new CommentDto
            {
                Username = "testuser",
                DisplayName = "Test User",
                Body = "Test comment",
                Image = "http://image.url"
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<Create.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CommentDto>.Success(commentDto));

            var clientProxyMock = new Mock<IClientProxy>();
            clientProxyMock
                .Setup(x => x.SendCoreAsync("ReceiveComment", It.IsAny<object[]>(), default))
                .Returns(Task.CompletedTask);

            var clientsMock = new Mock<IHubCallerClients>();
            clientsMock.Setup(c => c.Group(activityId.ToString()))
                       .Returns(clientProxyMock.Object);

            var contextMock = new Mock<HubCallerContext>();

            var hub = new ChatHub(mediatorMock.Object)
            {
                Clients = clientsMock.Object,
                Context = contextMock.Object
            };

            var command = new Create.Command
            {
                ActivityId = activityId,
                Body = "Test comment"
            };

            // Act
            await hub.SendComment(command);

            // Assert
            clientProxyMock.Verify(
                x => x.SendCoreAsync(
                    "ReceiveComment",
                    It.Is<object[]>(args => args.Length == 1 && args[0] == commentDto),
                    default
                ),
                Times.Once
            );
        }
    }
}
