using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    // CQRS command for deleting a user photo (if it's not the main photo)

    public class Delete
    {
        // Command carries the ID of the photo to delete
        public class Commend : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        // Handler that performs the deletion logic
        public class Handler : IRequestHandler<Commend, Result<Unit>>
        {
            public readonly DataContext _context;
            public readonly IPhotoAccessor _photoAccessor;
            private readonly IUserAccessor _userAccessor;

            // Constructor with dependencies
            public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _photoAccessor = photoAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Commend request, CancellationToken cancellationToken)
            {
                // Get current user including their photos
                var user = await _context.Users
                    .Include(p => p.Photos)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (user == null) return null;

                // Find the photo to delete by ID
                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);
                if (photo == null) return null;

                // Prevent deletion if the photo is the user's main photo
                if (photo.IsMain)
                    return Result<Unit>.Failure("you cannot delete your main photo");

                // Delete the photo from external storage (e.g., Cloudinary)
                var result = await _photoAccessor.DeletePhoto(photo.Id);
                if (result == null)
                    return Result<Unit>.Failure("problem deleting photo from Cloudinary");

                // Remove the photo from the database
                user.Photos.Remove(photo);

                // Save changes
                var success = await _context.SaveChangesAsync() > 0;
                if (success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("problem deleting photo from Api");
            }
        }
    }
}
