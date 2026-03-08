using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Profile.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Profiles.Queries;

public class GetUserActivities
{
    public class Query : IRequest<Result<List<UserActivityResponse>>>
    {
        public required string UserId { get; set; }
        public string Filter { get; set; } = "past";
    }
    
    public class Handler(
        AppDbContext dbContext,
        IMapper mapper): IRequestHandler<Query, Result<List<UserActivityResponse>>>
    {
        public async Task<Result<List<UserActivityResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (await dbContext.Users.FindAsync([request.UserId], cancellationToken) is not { } user)
            {
                return Result<List<UserActivityResponse>>.Failure("User not found", 404);
            }
            
            var today = DateTimeOffset.UtcNow;

            var activies = new List<UserActivityResponse>();

            switch (request.Filter)
            {
                case "past":
                    activies = await dbContext.Activities
                        .Where(a => a.Date < today)
                        .OrderByDescending(a => a.Date)
                        .ProjectTo<UserActivityResponse>(mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
                    break;
                case "future":
                    activies = await dbContext.Activities
                        .Where(a => a.Date >= today)
                        .OrderByDescending(a => a.Date)
                        .ProjectTo<UserActivityResponse>(mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
                    break;
                case "hosting":
                    activies = await dbContext.Activities
                        .Include(a => a.Attendees)
                        .Where(a => a.Attendees.Any(at => at.IsHost && at.UserId == user.Id))
                        .ProjectTo<UserActivityResponse>(mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
                    break;
            }
            
            return Result<List<UserActivityResponse>>.Success(activies);
        }
    }
}