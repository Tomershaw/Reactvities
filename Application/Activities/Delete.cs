using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public  class  Commend :IRequest
        {
            public Guid Id {get;  set;}

        }
        public class Handler : IRequestHandler<Commend>
        {
            private readonly DataContext _context;
            public Handler(DataContext  context )
            {
                _context =context;

            }


            public async Task<Unit> Handle(Commend request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);

                _context.Remove(activity);

                await _context.SaveChangesAsync();

                return Unit.Value; 
            }
        }
    }
}