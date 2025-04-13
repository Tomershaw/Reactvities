using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    // CQRS command to create a new activity.
    // This class handles the logic for adding a new activity to the database.

    public class Create
    {
        // Command object carrying the Activity data to be created.
        // Encapsulates the activity details provided by the client.
        public class Commend : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        // FluentValidation validator for the Commend object.
        // Ensures that the activity data being created meets the required validation rules.
        public class CommendValidator : AbstractValidator<Commend>
        {
            public CommendValidator()
            {
                // Reuses the ActivityValidator to validate the Activity model.
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        // Handler that processes the create command.
        // Implements IRequestHandler to handle the Commend object and return a Result<Unit>.
        public class Handler : IRequestHandler<Commend, Result<Unit>>
        {
            private readonly DataContext _context; // Database context for accessing activities.
            private readonly IUserAccessor _userAccessor; // Service to access the current user's information.

            // Constructor injecting the database context and user accessor service.
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            // Handles the creation logic:
            // - Retrieves the current user from the database.
            // - Adds the user as the host of the activity.
            // - Adds the activity to the database and saves changes.
            public async Task<Result<Unit>> Handle(Commend request, CancellationToken cancellationToken)
            {
                // Retrieve the current user based on their username.
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                // Create a new ActivityAttendee object to associate the user as the host.
                var attendees = new ActivityAttendee
                {
                    AppUser = user,
                    Activity = request.Activity,
                    IsHost = true,
                };

                // Add the host to the activity's attendees list.
                request.Activity.Attendees.Add(attendees);

                // Add the new activity to the database context.
                _context.Activities.Add(request.Activity);

                // Save changes to the database.
                var result = await _context.SaveChangesAsync() > 0;

                // If saving fails, return a failure result.
                if (!result) return Result<Unit>.Failure("failed to create activity");

                // Return a success result if the creation is successful.
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
