using Application.Core;
using Application.Interfaces;
using MediatR;
using Persistence.Contexts;

namespace Application.Profiles.Commands;

public class SetMainPhoto
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string PhotoId { get; set; }
    }

    public class Handler(AppDbContext dbContext, IUserAccessor userAccessor) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command command, CancellationToken cancellationToken)
        {
            if (await dbContext.Photos.FindAsync(command.PhotoId, cancellationToken) is not {} photo)
            {
                return Result<Unit>.Failure("Photo not found", 400);
            }
            
            var user = await userAccessor.GetUserAsync();
            user.ImageUrl = photo.Url;
            dbContext.Users.Update(user);
            
            var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;
            
            return result 
                ? Result<Unit>.Success(Unit.Value) 
                : Result<Unit>.Failure("Failed to save photo as Main one", 400);
        }
    }
}