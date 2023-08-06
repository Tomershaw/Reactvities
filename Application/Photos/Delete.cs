using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class Delete
    {
        public class Commend : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Commend, Result<Unit>>
        {
            public readonly DataContext _context ;
            public readonly IPhotoAccessor _photoAccessor ;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _photoAccessor = photoAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Commend request, CancellationToken cancellationToken)
            {
                var user  = await  _context.Users.Include(p => p.photos)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                 
                 if(user == null) return null;

                  var photo =user.photos.FirstOrDefault(x => x.Id == request.Id);

                  if(photo == null) return null;
                  
                  if(photo.IsMain) return Result<Unit>.Failure("you cannot delete your main photo ");

                  var result = await _photoAccessor.DeletePhoto(photo.Id);

                  if(result == null) return Result<Unit>.Failure("problem deleting photo from Cloudinary");

                  user.photos.Remove(photo);
                   var  success = await _context.SaveChangesAsync() > 0;

                   if(success) return Result<Unit>.Success(Unit.Value);

                   return Result<Unit>.Failure("problem deleting photo from Api ");

            }
        }
    }
}