using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    // CQRS command for setting a specific photo as the user's main photo

    public class SetMain
    {
        // Command carries the ID of the photo to set as main
        public class Commend : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        // Handler sets the specified photo as the user's main photo
        public class Handler : IRequestHandler<Commend, Result<Unit>>
        {
            public DataContext _context;
            public IUserAccessor _userAccessor;

            // Constructor injecting DB context and user accessor
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Commend request, CancellationToken cancellationToken)
            {
                // Retrieve the user and their photos
                var user = await _context.Users
                    .Include(p => p.Photos)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (user == null) return null;

                // Find the target photo by ID
                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);
                if (photo == null) return null;

                // Unset the current main photo, if any
                var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
                if (currentMain != null) currentMain.IsMain = false;

                // Set the selected photo as main
                photo.IsMain = true;

                // Save the change to the database
                var success = await _context.SaveChangesAsync() > 0;
                if (success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("problem setting main photo");
            }
        }
    }
}
