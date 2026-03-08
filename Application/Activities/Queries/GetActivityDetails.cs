using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Activity.Responses;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Activities.Queries;

public class GetActivityDetails
{
    public class Query : IRequest<Result<ActivityResponse>>
    {
        public required string Id { get; set; }
    }

    public class Handler(
        AppDbContext context, 
        IMapper mapper,
        IUserAccessor userAccessor) : IRequestHandler<Query, Result<ActivityResponse>>
    {
        public async Task<Result<ActivityResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities
                .ProjectTo<ActivityResponse>(mapper.ConfigurationProvider, 
                    new { currentUserId = userAccessor.GetUserId() })
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (activity == null)
            {
                return Result<ActivityResponse>.Failure("Activity not found", 404);
            }
            
            return Result<ActivityResponse>.Success(activity);
        }
    }
}