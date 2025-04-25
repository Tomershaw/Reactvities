using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    // CQRS command that handles toggling attendance for the current user.
    // Behavior:
    // - If user is not attending: adds them as attendee.
    // - If user is attending and not host: removes them.
    // - If user is host: toggles the cancellation status of the activity.

    public class UpdateAttendance
    {
        // Command object that contains the activity ID to toggle attendance for
        public class Commend : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; } // Activity ID
        }

        public class Handler : IRequestHandler<Commend, Result<Unit>>
        {
            public DataContext _context;
            public IUserAccessor _userAccessor;

            // Constructor injecting database context and user accessor
            public Handler(DataContext context, IUserAccessor userAccessor)
            {   
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Commend request, CancellationToken cancellationToken)
            {
                // Load the activity and its attendees (including related user data)
                var activity = await _context.Activities
                    .Include(a => a.Attendees)
                    .ThenInclude(u => u.AppUser)
                    .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (activity == null)
                    return null; // If activity doesn't exist

                // Get the currently logged-in user
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (user == null)
                    return null;

                // Get the username of the host
                var HostUsername = activity.Attendees
                    .FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

                // Check if the current user is already attending
                var attendance = activity.Attendees
                    .FirstOrDefault(x => x.AppUser.UserName == user.UserName);

                // Case 1: user is host and already attending → toggle IsCancelled
                if (attendance != null && HostUsername == user.UserName)
                    activity.IsCancelled = !activity.IsCancelled;

                // Case 2: user is attending but not the host → remove from attendees
                if (attendance != null && HostUsername != user.UserName)
                    activity.Attendees.Remove(attendance);

                // Case 3: user is not attending → add as attendee
                if (attendance == null)
                {
                    attendance = new ActivityAttendee
                    {
                        AppUser = user,
                        Activity = activity,
                        IsHost = false
                    };

                    activity.Attendees.Add(attendance);
                }

                // Save changes to database and return result
                var result = await _context.SaveChangesAsync() > 0;

                return result
                    ? Result<Unit>.Success(Unit.Value)
                    : Result<Unit>.Failure("problem updating attendance");
            }
        }
    }
}
