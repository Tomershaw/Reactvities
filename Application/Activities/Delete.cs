using Application.Core;
using MediatR;
using Persistence;

namespace Application.Activities
{
    // CQRS command for deleting an activity by ID.
    // This class handles the logic for removing an activity from the database.

    public class Delete
    {
        // Command object carrying the ID of the activity to delete.
        // Encapsulates the unique identifier of the activity to be removed.
        public class Commend : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        // Handler that processes the delete command.
        // Implements IRequestHandler to handle the Commend object and return a Result<Unit>.
        public class Handler : IRequestHandler<Commend, Result<Unit>>
        {
            private readonly DataContext _context; // Database context for accessing activities.

            // Constructor injecting the database context.
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handles the delete operation:
            // - Looks up the activity by ID.
            // - Removes it from the database.
            // - Saves changes and returns success/failure.
            public async Task<Result<Unit>> Handle(Commend request, CancellationToken cancellationToken)
            {
                // Find the activity by its ID.
                var activity = await _context.Activities.FindAsync(request.Id);

                // If the activity does not exist, return null.
                if (activity == null) return null;

                // Remove the activity from the database.
                _context.Remove(activity);

                // Save the changes to the database.
                var result = await _context.SaveChangesAsync() > 0;

                // If saving fails, return a failure result.
                if (!result) return Result<Unit>.Failure("failed to delete the activity");

                // Return a success result if the deletion is successful.
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
