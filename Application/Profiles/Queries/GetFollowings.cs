using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Profile.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Profiles.Queries;

public class GetFollowings
{
    public class Query : IRequest<Result<List<UserProfileResponse>>>
    {
        public string Predicate { get; set; } = "followers";
        public required string UserId { get; set; }
    }
    
    public class Handler(
        AppDbContext dbContext, 
        IMapper mapper,
        IUserAccessor userAccessor) : IRequestHandler<Query, Result<List<UserProfileResponse>>>
    {
        public async Task<Result<List<UserProfileResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (await userAccessor.GetUserAsync() is not { } currentUser)
            {
                return Result<List<UserProfileResponse>>.Failure("User not found", 400);
            }
            
            if (await dbContext.Users.FindAsync([request.UserId], cancellationToken) is not { } user)
            {
                return Result<List<UserProfileResponse>>.Failure("User not found", 400);
            }

            var profiles = new List<UserProfileResponse>();

            switch (request.Predicate)
            {
                case "followers":
                    profiles = await dbContext.UserFollowings
                        .Where(uf => uf.TargetId == request.UserId)
                        .Select(u => u.Observer)
                        .ProjectTo<UserProfileResponse>(mapper.ConfigurationProvider, 
                            new { currentUserId = currentUser.Id})
                        .ToListAsync(cancellationToken);
                    break;
                case "followings":
                    profiles = await dbContext.UserFollowings
                        .Where(uf => uf.ObserverId == request.UserId)
                        .Select(u => u.Target)
                        .ProjectTo<UserProfileResponse>(mapper.ConfigurationProvider,
                            new { currentUserId = currentUser.Id})
                        .ToListAsync(cancellationToken);
                    break;
            }
            
            return Result<List<UserProfileResponse>>.Success(profiles);
        }
    }
}