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

    public class List
    {
        // Query that includes the Activity ID to fetch comments for
        public class Query : IRequest<Result<List<CommentDto>>>
        {
            public Guid ActivityId { get; set; }
        }

        // Handler that executes the query and returns the list of comments
        public class Handler : IRequestHandler<Query, Result<List<CommentDto>>>
        {
            private readonly DataContext _context;
            public readonly IMapper _mapper;

            // Injects DB context and AutoMapper
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            // Executes the query:
            // - Filters comments by Activity ID
            // - Orders them by creation date (latest first)
            // - Projects to CommentDto using AutoMapper
            // - Returns the list wrapped in a Result object
            public async Task<Result<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var comments = await _context.Comments
                    .Where(x => x.Activity.Id == request.ActivityId)
                    .OrderByDescending(x => x.CreateAt)
                    .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return Result<List<CommentDto>>.Success(comments);
            }
        }
    }
}
