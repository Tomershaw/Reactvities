using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class SetMain
    {
        public class Commend : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Commend, Result<Unit>>
        {
            public DataContext _context;
            public IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Commend request, CancellationToken cancellationToken)
            {
               var user  = await _context.Users.Include(p => p.photos)
               .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

               if(user == null) return null;

               var photo = user.photos.FirstOrDefault(x => x.Id == request.Id);

               if(photo == null) return null;
                
                var currentMain = user.photos.FirstOrDefault(x => x.IsMain);

                if(currentMain != null) currentMain.IsMain = false; 

                photo.IsMain =true;

                var success = await _context.SaveChangesAsync() > 0;
                
                if(success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("problem setting main  photo ");


            }

        }
    }
}