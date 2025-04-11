using Application.Core;
using MediatR;
using Persistence;

namespace Application.Activities
{
    // CQRS command for deleting an activity by ID

    public class Delete
    {
        // Command object carrying the ID of the activity to delete
        public class Commend : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        // Handler that processes the delete command
        public class Handler : IRequestHandler<Commend, Result<Unit>>
        {
            private readonly DataContext _context;

            // Inject the application's database context
            public Handler(DataContext context)
            {
                _context = context;
            }

            // Handles the delete operation:
            // - looks up the activity by ID
            // - removes it from the database
            // - saves changes and returns success/failure
            public async Task<Result<Unit>> Handle(Commend request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);

                if (activity == null) return null;

                _context.Remove(activity);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("failed to delete the activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
