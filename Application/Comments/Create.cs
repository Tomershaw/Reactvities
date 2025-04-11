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

    public class Create
    {
        // The Command contains the comment body and the target activity ID.
        public class Command : IRequest<Result<CommentDto>>
        {
            public string Body { get; set; }
            public Guid ActivityId { get; set; }
        }

        // Validator for the Command: ensures the comment body is not empty.
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Body).NotEmpty();
            }

            // Handler that processes the creation of a comment
            public class Handler : IRequestHandler<Command, Result<CommentDto>>
            {
                private readonly DataContext _context;
                private readonly IMapper _mapper;
                private readonly IUserAccessor _userAccessor;

                // Constructor injecting required services: DB, Mapper, User accessor
                public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
                {
                    _userAccessor = userAccessor;
                    _mapper = mapper;
                    _context = context;
                }

                // Handles the actual comment creation logic
                public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
                {
                    // Load the activity by ID
                    var activity = await _context.Activities.FindAsync(request.ActivityId);
                    if (activity == null) return null;

                    // Load the current user with their photos
                    var user = await _context.Users
                        .Include(p => p.Photos)
                        .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                    // Build the comment entity
                    var comment = new Comment
                    {
                        Author = user,
                        Activity = activity,
                        Body = request.Body
                    };

                    // Add the comment to the activity's Comments collection
                    activity.Comments.Add(comment);

                    // Save changes and return result
                    var success = await _context.SaveChangesAsync() > 0;

                    if (success)
                        return Result<CommentDto>.Success(_mapper.Map<CommentDto>(comment));

                    return Result<CommentDto>.Failure("failed to add comment");
                }
            }
        }
    }
}
