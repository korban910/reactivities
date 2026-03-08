using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Profile.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Profiles.Queries;

public class GetProfile
{
    public class Query : IRequest<Result<UserProfileResponse>>
    {
        public required string UserId { get; set; }
    }

    public class Handler(
        AppDbContext dbContext, 
        IMapper mapper,
        IUserAccessor userAccessor) : IRequestHandler<Query, Result<UserProfileResponse>>
    {
        public async Task<Result<UserProfileResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var profile = await dbContext.Users
                .ProjectTo<UserProfileResponse>(mapper.ConfigurationProvider,
                    new { currentUserId = userAccessor.GetUserId() })
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (profile == null)
            {
                return Result<UserProfileResponse>.Failure("User not found", 404);
            }
            
            return Result<UserProfileResponse>.Success(profile);
        }
    }
}