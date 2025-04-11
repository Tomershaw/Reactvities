using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    // SignalR Hub that handles real-time chat messaging between users per activity
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        // Constructor injecting MediatR for CQRS-based request handling
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Handles incoming chat comments and broadcasts them to the appropriate group
        public async Task SendComment(Create.Command command)
        {
            // Send comment creation command to application layer
            var comment = await _mediator.Send(command);

            // Broadcast the created comment to all clients in the activity group
            await Clients.Group(command.ActivityId.ToString())
                .SendAsync("ReceiveComment", comment.Value);
        }

        // Runs when a client connects to the chat hub
        public override async Task OnConnectedAsync()
        {
            // Extract activityId from the query string of the connection request
            var HttpContext = Context.GetHttpContext();
            var activityId = HttpContext.Request.Query["activityId"];

            // Add the connecting client to the SignalR group based on activity ID
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

            // Load all previous comments for the activity and send to the newly connected client
            var result = await _mediator.Send(new List.Query { ActivityId = Guid.Parse(activityId) });
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}
