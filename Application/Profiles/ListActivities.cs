using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    // CQRS query for retrieving a list of activities related to a specific user
    // The activities can be filtered by "future", "past", or "hosting"

    public class ListActivities
    {
        // Query contains the username and a filter predicate
        public class Query : IRequest<Result<List<UserActivityDto>>>
        {
            public string Username { get; set; }      // The user to filter by
            public string Predicate { get; set; }     // Filter: "past", "hosting", or default ("future")
        }

        // Handler processes the query and applies the appropriate filter
        public class Handler : IRequestHandler<Query, Result<List<UserActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Build initial query: all activities the user attends
                var query = _context.ActivityAttendees
                    .Where(u => u.AppUser.UserName == request.Username)
                    .OrderBy(a => a.Activity.Date)
                    .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                    .AsQueryable();

                var today = DateTime.UtcNow;

                // Apply filter based on predicate
                query = request.Predicate switch
                {
                    "past" => query.Where(a => a.Date <= today),
                    "hosting" => query.Where(h => h.HostUsername == request.Username),
                    _ => query.Where(d => d.Date >= today) // default = future
                };

                // Execute query and return results
                var activities = await query.ToListAsync();
                return Result<List<UserActivityDto>>.Success(activities);
            }
        }
    }
}
