using System.Runtime.InteropServices;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
    /// <summary>
    /// CQRS command for toggling the "follow" state between the current user and a target user.
    /// </summary>
    public class FollowToggle
    {
        /// <summary>
        /// Command contains the username of the user to follow or unfollow.
        /// </summary>
        public class Command : IRequest<Result<Unit>>
        {
            public string TargetUsername { get; set; }
        }

        /// <summary>
        /// Handler that processes the follow/unfollow logic.
        /// </summary>
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context; // Database context for accessing user and follow data
            private readonly IUserAccessor _userAccessor; // Service to get the current user's username

            /// <summary>
            /// Constructor: Injects dependencies (DataContext, IUserAccessor).
            /// </summary>
            /// <param name="context">The database context.</param>
            /// <param name="userAccessor">The user accessor service.</param>
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            /// <summary>
            /// Handles the follow/unfollow operation.
            /// </summary>
            /// <param name="request">The command containing the target username.</param>
            /// <param name="cancellationToken">Token to cancel the operation.</param>
            /// <returns>A Result indicating success or failure.</returns>
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Get the current user (observer)
                var observer = await _context.Users
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                // Get the target user to follow/unfollow
                var target = await _context.Users
                    .FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);

                // If the target user doesn't exist, return null
                if (target == null) return null;

                // Check if a follow relationship already exists
                var Following = await _context.UserFollowings.FindAsync(observer.Id, target.Id);

                if (Following == null)
                {
                    // If not following yet, create a new follow relationship
                    Following = new UserFollowing
                    {
                        Observer = observer,
                        Target = target
                    };
                    _context.UserFollowings.Add(Following);
                }
                else
                {
                    // If already following, remove the relationship (unfollow)
                    _context.UserFollowings.Remove(Following);
                }

                // Save changes to the database and return the result
                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Failed to update following");
            }
        }
    }
}
