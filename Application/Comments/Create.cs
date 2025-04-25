using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments
{
    // CQRS command for creating a new comment related to a specific activity.
    // This class handles the logic for adding a new comment to the database.

    public class Create
    {
        // The Command contains the comment body and the target activity ID.
        // This is the input for the command handler.
        public class Command : IRequest<Result<CommentDto>>
        {
            // The content or text of the comment being created.
            public string Body { get; set; }

            // The unique identifier of the activity to which the comment belongs.
            public Guid ActivityId { get; set; }
        }

        // Validator for the Command: ensures the comment body is not empty.
        // This enforces basic validation rules for the input.
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Body).NotEmpty(); // Ensure the comment body is not empty.
            }
        }

        // Handler that processes the creation of a comment.
        // This contains the business logic for adding a new comment.
        public class Handler : IRequestHandler<Command, Result<CommentDto>>
        {
            private readonly DataContext _context; // Database context for accessing data.
            private readonly IMapper _mapper; // AutoMapper for mapping entities to DTOs.
            private readonly IUserAccessor _userAccessor; // Service for accessing the current user.

            // Constructor injecting required services: database context, AutoMapper, and user accessor.
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            // Handles the actual comment creation logic.
            public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Load the activity by ID.
                var activity = await _context.Activities.FindAsync(request.ActivityId);
                if (activity == null)
                return Result<CommentDto>.Failure("Activity not found"); // Return null if the activity is not found.
                

                // Load the current user with their photos.
                var user = await _context.Users
                    .Include(p => p.Photos) // Include user photos for mapping.
                    .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                // Build the comment entity.
                var comment = new Comment
                {
                    Author = user, // Set the author to the current user.
                    Activity = activity, // Associate the comment with the activity.
                    Body = request.Body // Set the comment body.
                };

                // Add the comment to the activity's Comments collection.
                activity.Comments.Add(comment);

                // Save changes to the database and return the result.
                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                    return Result<CommentDto>.Success(_mapper.Map<CommentDto>(comment)); // Map to CommentDto and return success.

                return Result<CommentDto>.Failure("failed to add comment"); // Return failure if saving changes fails.
            }
        }
    }
}
