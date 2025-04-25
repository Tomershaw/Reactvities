using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Comments;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;
using API.SignalR;

namespace Project.Tests.API.SignalR
{
    public class ChatHubLogicTests
    {
        [Fact]
        public async Task SendComment_Should_Invoke_Mediator_And_Broadcast_Result()
        {
            // Arrange
            var commentDto = new CommentDto
            {
                Id = 1,
                Body = "Test comment",
                Username = "testuser",
                DisplayName = "Test User",
                Image = "http://image.com/photo.jpg"
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<Create.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CommentDto>.Success(commentDto));

            var clientsMock = new Mock<IHubCallerClients>();
            var groupMock = new Mock<IClientProxy>();
            clientsMock.Setup(c => c.Group(It.IsAny<string>())).Returns(groupMock.Object);

            var contextMock = new Mock<HubCallerContext>();

            var hub = new ChatHub(mediatorMock.Object)
            {
                Clients = clientsMock.Object,
                Context = contextMock.Object
            };

            var command = new Create.Command
            {
                ActivityId = Guid.NewGuid(),
                Body = "Test comment"
            };

            // Act
            await hub.SendComment(command);

            // Assert
            clientsMock.Verify(c => c.Group(command.ActivityId.ToString()), Times.Once);
            groupMock.Verify(g => g.SendCoreAsync(
                "ReceiveComment",
                It.Is<object[]>(o => o.Length == 1 && o[0] == commentDto),
                default), Times.Once);
        }
    }
}
