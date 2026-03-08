using Application.Core;
using Application.Interfaces;
using MediatR;
using Persistence.Contexts;

namespace Application.Profiles.Commands;

public class DeletePhoto
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string PhotoId { get; set; }
    }

    public class Handler(
        IUserAccessor userAccessor, 
        AppDbContext dbContext,
        IPhotoService photoService) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command command, CancellationToken cancellationToken)
        {
            var user = await userAccessor.GetUserWithPhotosAsync();
            
            var photo = user.Photos.SingleOrDefault(p => p.Id == command.PhotoId);

            if (photo == null)
            {
                return Result<Unit>.Failure("Photo not found", 404);
            }

            if (photo.UserId != user.Id)
            {
                return Result<Unit>.Failure("Photo does not belong to this user", 404);
            }

            if (user.ImageUrl == photo.Url)
            {
                return Result<Unit>.Failure("Main photo can not be deleted", 404);
            }
            
            await photoService.DeletePhoto(photo.PublicId);
            
            user.Photos.Remove(photo);
            
            var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;
            
            return result 
                ? Result<Unit>.Success(Unit.Value) 
                : Result<Unit>.Failure("Failed to delete photo", 404);
        }
    }
}