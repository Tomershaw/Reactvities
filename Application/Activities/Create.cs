using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    // CQRS command to create a new activity

    public class Create
    {
        // Command object carrying the Activity data to be created
        public class Commend : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        // FluentValidation validator for the Commend object
        // Uses ActivityValidator to validate the Activity model
        public class CommendValidator : AbstractValidator<Commend>
        {
            public CommendValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        // Handles the command to create an activity in the database
        public class Handler : IRequestHandler<Commend, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            // Injects the database context and user accessor service
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            // Handles the creation logic:
            // - gets the current user
            // - adds the user as host to the activity
            // - adds the activity to the database
            public async Task<Result<Unit>> Handle(Commend request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                var attendees = new ActivityAttendee
                {
                    AppUser = user,
                    Activity = request.Activity,
                    IsHost = true,
                };

                request.Activity.Attendees.Add(attendees);
                _context.Activities.Add(request.Activity);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("faild to create activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
