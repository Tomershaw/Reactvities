using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    // CQRS query for retrieving activity details by ID.
    // This class handles the logic for fetching a specific activity's details.

    public class Detalis
    {
        // Query object containing the ID of the activity to fetch.
        // Encapsulates the unique identifier of the activity being requested.
        public class Query : IRequest<Result<ActivityDto>>
        {
            public Guid Id { get; set; }
        }

        // Handler that processes the query and returns the activity DTO.
        // Implements IRequestHandler to handle the Query object and return a Result<ActivityDto>.
        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
        {
            private readonly DataContext _context; // Database context for accessing activities.
            private readonly IMapper _mapper; // AutoMapper for mapping entities to DTOs.
            private readonly IUserAccessor _userAccessor; // Service to access the current user's information.

            // Constructor injecting the database context, AutoMapper, and user accessor.
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            // Handles the query:
            // - Uses ProjectTo for efficient AutoMapper projection to ActivityDto.
            // - Supplies the current user's username to the mapping configuration.
            // - Finds the activity by ID and returns it.
            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
              {
               var activity = await _context.Activities
               .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() })
               .FirstOrDefaultAsync(x => x.Id == request.Id);

               if (activity == null)
               return Result<ActivityDto>.Failure("Activity not found");

                return Result<ActivityDto>.Success(activity);
             }

        }
    }
}
