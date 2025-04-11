using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    // CQRS command for uploading a photo and adding it to the user's profile

    public class Add
    {
        // Command carries the uploaded file
        public class Command : IRequest<Result<Photo>>
        {
            public IFormFile File { get; set; } // Uploaded photo file from form
        }

        // Handler performs the photo upload and updates the user entity
        public class Handler : IRequestHandler<Command, Result<Photo>>
        {
            public readonly DataContext _context;
            private readonly IPhotoAccessor _photoAccessor;
            public readonly IUserAccessor _userAccessor;

            // Injects database, photo accessor, and user accessor services
            public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _photoAccessor = photoAccessor;
                _context = context;
            }

            public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Retrieve the current user including their photos
                var user = await _context.Users
                    .Include(p => p.Photos)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (user == null) return null;

                // Upload photo to external storage (e.g., Cloudinary)
                var PhotoUploadResult = await _photoAccessor.AddPhoto(request.File);

                // Create a new photo entity using the returned URL and ID
                var photo = new Photo
                {
                    Url = PhotoUploadResult.Url,
                    Id = PhotoUploadResult.PublicId
                };

                // Set the photo as main if the user has no main photo yet
                if (!user.Photos.Any(x => x.IsMain)) photo.IsMain = true;

                // Add the photo to the user's collection
                user.Photos.Add(photo);

                // Save changes to the database
                var result = await _context.SaveChangesAsync() > 0;

                if (result) return Result<Photo>.Success(photo);
                return Result<Photo>.Failure("problem adding photo");
            }
        }
    }
}
// Compare this snippet from Application/Photos/PhotoUploadResult.cs: