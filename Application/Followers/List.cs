using Application.Core;
using Application.Interfaces;
using Application.Profiles;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
    /// <summary>
    /// CQRS query for retrieving a list of followers or followings for a specific user.
    /// </summary>
    public class List
    {
        /// <summary>
        /// Query contains the username and the predicate: either "followers" or "following".
        /// </summary>
        public class Query : IRequest<Result<List<Profiles.Profile>>>
        {
            public string Predicate { get; set; } // "followers" or "following"
            public string Username { get; set; }  // The target user to inspect
        }

        /// <summary>
        /// Handler that executes the query logic.
        /// </summary>
        public class Handler : IRequestHandler<Query, Result<List<Profiles.Profile>>>
        {
            private readonly DataContext _context; // Database context for accessing user and follow data
            private readonly IMapper _mapper; // Mapper for converting entities to DTOs
            private readonly IUserAccessor _userAccessor; // Service to get the current user's username

            /// <summary>
            /// Constructor: Injects dependencies (DataContext, IMapper, IUserAccessor).
            /// </summary>
            /// <param name="context">The database context.</param>
            /// <param name="mapper">The AutoMapper instance.</param>
            /// <param name="userAccessor">The user accessor service.</param>
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            /// <summary>
            /// Handles the query to retrieve followers or followings.
            /// </summary>
            /// <param name="request">The query containing the username and predicate.</param>
            /// <param name="cancellationToken">Token to cancel the operation.</param>
            /// <returns>A Result containing the list of profiles.</returns>
            public async Task<Result<List<Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profiles = new List<Profiles.Profile>();

                // Based on the predicate, return either list of followers or following
                switch (request.Predicate)
                {
                    case "followers":
                        profiles = await _context.UserFollowings
                            .Where(x => x.Target.UserName == request.Username)
                            .Select(u => u.Observer)
                            .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider,
                                new { currentUsername = _userAccessor.GetUsername() })
                            .ToListAsync();
                        break;

                    case "following":
                        profiles = await _context.UserFollowings
                            .Where(x => x.Observer.UserName == request.Username)
                            .Select(u => u.Target)
                            .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider,
                                new { currentUsername = _userAccessor.GetUsername() })
                            .ToListAsync();
                        break;
                }

                return Result<List<Profiles.Profile>>.Success(profiles);
            }
        }
    }
}
