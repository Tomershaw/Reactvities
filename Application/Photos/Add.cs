using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class Add
    {
        public class Command : IRequest<Result<Photo>>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Photo>>
        {
            public readonly DataContext _context ;
            private readonly IPhotoAccessor _photoAccessor;
            public readonly IUserAccessor _userAccessor ;
            public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _photoAccessor = photoAccessor;
                _context = context;
            }

            public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(p => p.photos)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if(user == null ) return null;

                 var PhotoUploadResult = await _photoAccessor.AddPhoto(request.File);   

                 var photo =new Photo
                 {
                    Url =PhotoUploadResult.Url,
                    Id = PhotoUploadResult.PublicId
                 };

                 if (!user.photos.Any(x => x.IsMain)) photo.IsMain = true;
    
                 user.photos.Add(photo);

                 var result  =await _context.SaveChangesAsync() > 0;

                 if (result) return Result<Photo>.Success(photo);
                  return Result<Photo>.Failure("problem adding photo");

            }
        }
    }
}