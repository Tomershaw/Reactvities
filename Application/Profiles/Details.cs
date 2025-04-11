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
        // Query contains the username to fetch the profile for
        public class Query : IRequest<Result<Profile>>
        {
            public string Username { get; set; }
        }

        // Handler retrieves and maps the user data to a Profile DTO
        public class Handler : IRequestHandler<Query, Result<Profile>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            // Constructor injecting DB context, AutoMapper, and user accessor
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Query and map the user from the database to a Profile DTO
                var user = await _context.Users
                    .ProjectTo<Profile>(
                        _mapper.ConfigurationProvider,
                        new { currentUsername = _userAccessor.GetUsername() }
                    )
                    .SingleOrDefaultAsync(x => x.UserName == request.Username);

                // If user not found, return null
                if (user == null) return null;

                // Wrap and return the result
                return Result<Profile>.Success(user);
            }
        }
    }
}
