using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.SignalR;
using Application.Comments;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace Project.Tests.API.SignalR
{
    public class ChatHubConnectionTests
    {
        [Fact]
        public async Task OnConnectedAsync_Should_Add_User_To_Group_And_Load_Comments()
        {
            // Arrange
            var activityId = Guid.NewGuid();

            var expectedComments = new List<CommentDto>
            {
                new CommentDto { Body = "Hello", Username = "testuser" }
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<List.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<List<CommentDto>>.Success(expectedComments));

            // Create HttpContext with query parameters
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Query = new QueryCollection(
                new Dictionary<string, StringValues>
                {
                    { "activityId", activityId.ToString() }
                });

            // Create a mock object for HubCallerContext
            var hubCallerContextMock = new Mock<HubCallerContext>();
            hubCallerContextMock.Setup(c => c.ConnectionId).Returns("test-connection");
            
            // Create a mock Features collection
            var featuresMock = new Mock<IFeatureCollection>();
            
            // Create mock HttpContextFeature that returns our httpContext
            var httpContextFeatureMock = new Mock<IHttpContextFeature>();
            httpContextFeatureMock.Setup(f => f.HttpContext).Returns(httpContext);
            
            // Connect the features collection to return our HttpContextFeature
            featuresMock.Setup(f => f.Get<IHttpContextFeature>())
                .Returns(httpContextFeatureMock.Object);
            
            // Set up the Features property on the HubCallerContext
            hubCallerContextMock.Setup(c => c.Features).Returns(featuresMock.Object);

            var clientProxyMock = new Mock<ISingleClientProxy>();
            clientProxyMock
                .Setup(x => x.SendCoreAsync(
                    "LoadComments",
                    It.Is<object[]>(o => o != null && o.Length == 1 && o[0] == expectedComments),
                    default))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var clientsMock = new Mock<IHubCallerClients>();
            clientsMock.Setup(x => x.Caller).Returns(clientProxyMock.Object);

            var groupManagerMock = new Mock<IGroupManager>();
            groupManagerMock
                .Setup(x => x.AddToGroupAsync("test-connection", activityId.ToString(), default))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var hub = new ChatHub(mediatorMock.Object)
            {
                Context = hubCallerContextMock.Object,
                Clients = clientsMock.Object,
                Groups = groupManagerMock.Object
            };

            // Act
            await hub.OnConnectedAsync();

            // Assert
            groupManagerMock.Verify(x => x.AddToGroupAsync("test-connection", activityId.ToString(), default), Times.Once);
            clientProxyMock.Verify(x => x.SendCoreAsync(
                "LoadComments",
                It.Is<object[]>(o => o != null && o.Length == 1 && o[0] == expectedComments),
                default), Times.Once);
        }
    }
}