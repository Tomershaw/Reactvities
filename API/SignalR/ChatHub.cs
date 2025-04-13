using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    /// <summary>
    /// SignalR Hub that handles real-time chat messaging between users per activity.
    /// </summary>
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor: Injects MediatR for handling CQRS-based requests.
        /// </summary>
        /// <param name="mediator">The MediatR instance for sending commands and queries.</param>
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Handles incoming chat comments and broadcasts them to the appropriate group.
        /// </summary>
        /// <param name="command">The command containing the comment details.</param>
        public async Task SendComment(Create.Command command)
        {
            // Send the comment creation command to the application layer.
            var comment = await _mediator.Send(command);

            // Broadcast the created comment to all clients in the activity group.
            await Clients.Group(command.ActivityId.ToString())
                .SendAsync("ReceiveComment", comment.Value);
        }

        /// <summary>
        /// Runs when a client connects to the chat hub.
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            // Extract the activity ID from the query string of the connection request.
            var HttpContext = Context.GetHttpContext();
            var activityId = HttpContext.Request.Query["activityId"];

            // Add the connecting client to the SignalR group based on the activity ID.
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

            // Load all previous comments for the activity and send them to the newly connected client.
            var result = await _mediator.Send(new List.Query { ActivityId = Guid.Parse(activityId) });
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}
