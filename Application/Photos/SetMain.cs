using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    // CQRS command for setting a specific photo as the user's main photo
    // This operation ensures that only one photo is marked as the main photo at a time.

    public class SetMain
    {
        // Command carries the ID of the photo to set as main
        public class Commend : IRequest<Result<Unit>>
        {
            // ID of the photo to be set as the main photo
            public string Id { get; set; }
        }

        // Handler sets the specified photo as the user's main photo
        public class Handler : IRequestHandler<Commend, Result<Unit>>
        {
            // Database context for accessing user and photo data
            public DataContext _context;

            // Service to access the currently logged-in user's information
            public IUserAccessor _userAccessor;

            // Constructor injecting DB context and user accessor
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Commend request, CancellationToken cancellationToken)
            {
                // Retrieve the user and their photos from the database
                var user = await _context.Users
                    .Include(p => p.Photos)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                // Return null if the user is not found
                if (user == null) return null;

                // Find the target photo by ID
                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

                // Return null if the photo is not found
                if (photo == null) return null;

                // Unset the current main photo, if any
                var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
                if (currentMain != null) currentMain.IsMain = false;

                // Set the selected photo as the main photo
                photo.IsMain = true;

                // Save the changes to the database
                var success = await _context.SaveChangesAsync() > 0;

                // Return success or failure based on the database operation result
                if (success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("problem setting main photo");
            }
        }
    }
}
