using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Commend : IRequest<Result<Unit>>
        {
            public Activity  Activity {get; set;}
        }

        public  class  CommendValidator:AbstractValidator<Commend>
        {
            public CommendValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }

        }

        public class Handler : IRequestHandler<Commend,Result<Unit>>
        {
        private readonly DataContext _context ;
            public Handler(DataContext context)
            {
            _context = context;
            }

            public async Task<Result<Unit>> Handle(Commend request, CancellationToken cancellationToken)
            {
                _context.Activities.Add(request.Activity);

                var result=await _context.SaveChangesAsync() > 0;

                if(!result) return Result<Unit>.Failure("faild to  create activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}