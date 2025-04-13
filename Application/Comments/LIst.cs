using System.Linq;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments
{
    // CQRS query for retrieving all comments associated with a specific activity.
    // This class handles the logic for fetching and returning comments.

    public class List
    {
        // Query object that includes the Activity ID to fetch comments for.
        // This is the input for the query handler.
        public class Query : IRequest<Result<List<CommentDto>>>
        {
            // The unique identifier of the activity whose comments are being fetched.
            public Guid ActivityId { get; set; }
        }

        // Handler that executes the query and returns the list of comments.
        // This contains the business logic for fetching and transforming the data.
        public class Handler : IRequestHandler<Query, Result<List<CommentDto>>>
        {
            private readonly DataContext _context; // Database context for accessing data.
            public readonly IMapper _mapper; // AutoMapper for mapping entities to DTOs.

            // Constructor to inject dependencies: database context and AutoMapper.
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            // Executes the query:
            // - Filters comments by Activity ID.
            // - Orders them by creation date (latest first).
            // - Projects to CommentDto using AutoMapper.
            // - Returns the list wrapped in a Result object.
            public async Task<Result<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var comments = await _context.Comments
                    .Where(x => x.Activity.Id == request.ActivityId) // Filter by Activity ID.
                    .OrderByDescending(x => x.CreateAt) // Order by creation date (descending).
                    .ProjectTo<CommentDto>(_mapper.ConfigurationProvider) // Map to CommentDto.
                    .ToListAsync(); // Execute query and fetch results.

                // Wrap the result in a success response and return.
                return Result<List<CommentDto>>.Success(comments);
            }
        }
    }
}
