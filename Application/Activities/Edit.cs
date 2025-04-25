using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    // CQRS command for editing an existing activity.
    // This class handles the logic for updating an activity's details in the database.

    public class Edit
    {
        // Command object carrying the updated activity data.
        // This object encapsulates the activity to be updated.
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        // FluentValidation rules for the edit command.
        // Ensures that the activity data being updated meets the required validation rules.
        public class CommendValidator : AbstractValidator<Command>
        {
            public CommendValidator()
            {
                // Reuses the same validation rules defined for creating an activity.
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        // Handler that processes the edit command.
        // Implements IRequestHandler to handle the Commend object and return a Result<Unit>.
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context; // Database context for accessing activities.
            private readonly IMapper _mapper; // AutoMapper for mapping updated data onto the entity.

            // Constructor injecting the database context and AutoMapper.
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            // Handles the update operation:
            // - Retrieves the existing activity from the database.
            // - Uses AutoMapper to map changes from the request onto the entity.
            // - Saves the changes to the database and returns the result.
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Find the activity by its ID.
                var activity = await _context.Activities.FindAsync(request.Activity.Id);

                // If the activity does not exist, return null.
                if (activity == null)
                   return Result<Unit>.Failure("Activity not found");
                // Map the updated activity data onto the existing entity.
                _mapper.Map(request.Activity, activity);

                // Save the changes to the database.
                var result = await _context.SaveChangesAsync() > 0;

                // If saving fails, return a failure result.
                if (!result) return Result<Unit>.Failure("failed to update activity");

                // Return a success result if the update is successful.
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
