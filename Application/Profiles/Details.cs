using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    // CQRS query for retrieving the profile of a specific user by username
    public class Details
    {
        // Query object containing the username to fetch the profile for
        public class Query : IRequest<Result<Profile>>
        {
            // The username of the user whose profile is being retrieved
            public string Username { get; set; }
        }

        // Handler retrieves and maps the user data to a Profile DTO
        public class Handler : IRequestHandler<Query, Result<Profile>>
        {
            private readonly DataContext _context; // Database context for accessing user data
            private readonly IMapper _mapper;      // AutoMapper for mapping entities to DTOs
            private readonly IUserAccessor _userAccessor; // Accessor for retrieving the current user's username

            // Constructor to inject dependencies
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            // Handles the query to retrieve the user's profile
            public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Query the database to retrieve the user and map it to a Profile DTO
                var user = await _context.Users
                    .ProjectTo<Profile>(
                        _mapper.ConfigurationProvider, // AutoMapper configuration
                        new { currentUsername = _userAccessor.GetUsername() } // Pass current username for mapping
                    )
                    .SingleOrDefaultAsync(x => x.UserName == request.Username); // Filter by username

                // If the user is not found, return null
                if (user == null)
                return Result<Profile>.Failure("Profile not found");

                // Wrap the result in a success object and return it
                return Result<Profile>.Success(user);
            }
        }
    }
}
