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
        // Query object containing the username and filter predicate
        public class Query : IRequest<Result<List<UserActivityDto>>>
        {
            // The username of the user whose activities are being queried
            public string Username { get; set; }

            // The filter predicate to determine which activities to retrieve
            // Options: "past", "hosting", or default ("future")
            public string Predicate { get; set; }
        }

        // Handler processes the query and retrieves the filtered list of activities
        public class Handler : IRequestHandler<Query, Result<List<UserActivityDto>>>
        {
            private readonly DataContext _context; // Database context for accessing data
            private readonly IMapper _mapper;      // AutoMapper for mapping entities to DTOs

            // Constructor to inject dependencies
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            // Handles the query and applies the appropriate filter
            public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Build the initial query to retrieve all activities the user is attending
                var query = _context.ActivityAttendees
                    .Where(u => u.AppUser.UserName == request.Username) // Filter by username
                    .OrderBy(a => a.Activity.Date)                     // Order by activity date
                    .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider) // Map to DTO
                    .AsQueryable();

                var today = DateTime.UtcNow; // Current date and time in UTC

                // Apply the filter based on the predicate
                query = request.Predicate switch
                {
                    "past" => query.Where(a => a.Date <= today), // Activities in the past
                    "hosting" => query.Where(h => h.HostUsername == request.Username), // Activities hosted by the user
                    _ => query.Where(d => d.Date >= today) // Default: future activities
                };

                // Execute the query and retrieve the results as a list
                var activities = await query.ToListAsync();

                // Return the results wrapped in a success result object
                return Result<List<UserActivityDto>>.Success(activities);
            }
        }
    }
}
