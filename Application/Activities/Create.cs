using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Commend : IRequest
        {
            public Activity  Activity {get; set;}
        }

        public class Handler : IRequestHandler<Commend>
        {
        private readonly DataContext _context ;
            public Handler(DataContext context)
            {
            _context = context;
            }

            public async Task<Unit> Handle(Commend request, CancellationToken cancellationToken)
            {
                _context.Activities.Add(request.Activity);

                await _context.SaveChangesAsync();

                return Unit.Value; 
            }
        }
    }
}