using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    // CQRS command for editing an existing activity

    public class Edit
    {
        // Command object carrying the updated activity data
        public class Commend : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        // FluentValidation rules for the edit command
        public class CommendValidator : AbstractValidator<Commend>
        {
            public CommendValidator()
            {
                // Reuses the same validation rules defined for creating an activity
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        // Handler that processes the edit command
        public class Handler : IRequestHandler<Commend, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            // Constructor injecting the database context and AutoMapper
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            // Handles the update:
            // - Retrieves the existing activity from the database
            // - Uses AutoMapper to map changes onto the entity
            // - Saves the changes and returns the result
            public async Task<Result<Unit>> Handle(Commend request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Activity.Id);

                if (activity == null) return null;

                _mapper.Map(request.Activity, activity);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("failed to update activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
