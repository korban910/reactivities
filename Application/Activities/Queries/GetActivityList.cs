using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Activity.Requests;
using Contracts.Activity.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Activities.Queries;

public class GetActivityList
{
    public class Query : IRequest<Result<PagedList<ActivityResponse, DateTimeOffset?>>>
    {
        public required ActivityParamsRequest Params { get; set; }
    }

    public class Handler(
        AppDbContext context, 
        IMapper mapper,
        IUserAccessor userAccessor) : IRequestHandler<Query, Result<PagedList<ActivityResponse, DateTimeOffset?>>>
    {
        public async Task<Result<PagedList<ActivityResponse, DateTimeOffset?>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = context.Activities
                .OrderBy(x => x.Date)
                .Where(x => x.Date >= request.Params.StartDate) // request.Params.StartDate, request.Params.Cursor
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Params.Filter))
            {
                query = request.Params.Filter switch
                {
                    "isGoing" => query.Where(x =>
                        x.Attendees.Any(a => a.UserId == userAccessor.GetUserId())),
                    "isHost" => query.Where(x =>
                        x.Attendees.Any(a => a.IsHost && a.UserId == userAccessor.GetUserId())),
                    _ => query
                };
            }

            var projectedActivities = query
                .ProjectTo<ActivityResponse>(mapper.ConfigurationProvider,
                new { currentUserId = userAccessor.GetUserId() });
                
            var activities = await projectedActivities
                .Take(request.Params.PageSize + 1)
                .ToListAsync(cancellationToken);
            
            DateTimeOffset? nextCursor = null;
            if (activities.Count > request.Params.PageSize)
            {
                nextCursor = activities.Last().Date;
                activities.RemoveAt(activities.Count - 1);
            }
            
            return Result<PagedList<ActivityResponse, DateTimeOffset?>>.Success(
                new PagedList<ActivityResponse, DateTimeOffset?>
                {
                    Items = activities,
                    NextCursor =  nextCursor
                });
        }
    }
}