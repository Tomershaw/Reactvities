using MediatR;
using Domain;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Interfaces;
using System.Linq;

namespace Application.Activities
{
    // CQRS query to retrieve a paginated, filtered list of activities.
    // This class handles the logic for querying activities based on user preferences,
    // such as filtering by date, checking if the user is attending or hosting, and pagination.

    public class List
    {
        // Query object containing parameters for filtering and pagination.
        // Includes ActivityParams, which defines filters like "IsGoing" and "IsHost".
        public class Query : IRequest<Result<PagedList<ActivityDto>>>
        {
            public ActivityParams Params { get; set; }
        }

        // Handler that processes the query and returns a paginated result.
        // Implements IRequestHandler to handle the Query object and return a Result containing a PagedList of ActivityDto.
        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
        {
            private readonly DataContext _context; // Database context for accessing activities.
            private readonly IMapper _mapper; // AutoMapper for mapping entities to DTOs.
            private readonly IUserAccessor _userAccessor; // Service to access the current user's information.

            // Constructor to inject dependencies: database context, AutoMapper, and user accessor.
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            // Main handler logic:
            // - Filters activities by date (only future activities are shown).
            // - Applies additional filters based on user preferences: "IsGoing" or "IsHost".
            // - Uses AutoMapper's ProjectTo for efficient projection to ActivityDto.
            // - Returns a paginated result using a custom paging helper.
            public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Base query: filters activities by start date and orders them by date.
                var query = _context.Activities
                    .Where(d => d.Date >= request.Params.StartDate)
                    .OrderBy(d => d.Date)
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                        new { currentUsername = _userAccessor.GetUsername() })
                    .AsQueryable();

                // If the user is attending activities but not hosting, filter by attendees.
                if (request.Params.IsGoing && !request.Params.IsHost)
                {
                    query = query.Where(x => x.Attendees.Any(a => a.UserName == _userAccessor.GetUsername()));
                }

                // If the user is hosting activities but not attending, filter by host username.
                if (request.Params.IsHost && !request.Params.IsGoing)
                {
                    query = query.Where(x => x.HostUsername == _userAccessor.GetUsername());
                }

                // Return the paginated result using the custom PagedList helper.
                return Result<PagedList<ActivityDto>>.Success(
                    await PagedList<ActivityDto>.CreateAsync(
                        query,
                        request.Params.PageNumber,
                        request.Params.PageSize
                    )
                );
            }
        }
    }
}
