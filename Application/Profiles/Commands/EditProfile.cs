using Application.Core;
using Application.Interfaces;
using MediatR;
using Persistence.Contexts;

namespace Application.Profiles.Commands;

public class EditProfile
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string DisplayName { get; set; }
        public string? Bio { get; set; }
    }

    public class Handler(AppDbContext dbContext, IUserAccessor userAccessor) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command command, CancellationToken cancellationToken)
        {
            if (await userAccessor.GetUserAsync() is not { } user)
            {
                return Result<Unit>.Failure("User not found", 400);
            }

            if (user.DisplayName == command.DisplayName && user.Bio == command.Bio)
            {
                return Result<Unit>.Failure("No changes to save", 400);
            }
            
            user.DisplayName = command.DisplayName;
            user.Bio = command.Bio;
            
            dbContext.Update(user);
            var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;
            return result ? Result<Unit>.Success(Unit.Value) :  Result<Unit>.Failure("Failed to update profile", 400);
        }
    }
}