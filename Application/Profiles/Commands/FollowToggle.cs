using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Persistence.Contexts;

namespace Application.Profiles.Commands;

public class FollowToggle
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string TargetUserId { get; set; }
    }

    public class Handler(AppDbContext dbContext, IUserAccessor userAccessor) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await userAccessor.GetUserAsync() is not { } user)
            {
                return Result<Unit>.Failure("User not found", 400); 
            }

            if (await dbContext.Users.FindAsync([request.TargetUserId], cancellationToken) is not { } targetUser)
            {
                return Result<Unit>.Failure("Target user not found", 400);
            }
            
            var following = await dbContext.UserFollowings.FindAsync([user.Id, targetUser.Id], cancellationToken);

            if (following == null)
            {
                var userFollowing = new UserFollowing
                {
                    TargetId = targetUser.Id,
                    ObserverId = user.Id
                };
            
                await dbContext.UserFollowings.AddAsync(userFollowing, cancellationToken);
            }
            else
            {
                dbContext.UserFollowings.Remove(following);
            }
            
            var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;
            
            return result ?
                Result<Unit>.Success(Unit.Value) :
                Result<Unit>.Failure("Failed to toggle follower changes", 400);
        }
    }
}