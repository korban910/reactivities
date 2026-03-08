using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Contracts.Profile.Responses;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence.Contexts;

namespace Application.Profiles.Commands;

public class AddPhoto
{
    public class Command : IRequest<Result<PhotoResponse>>
    {
        public required IFormFile File { get; set; }
    }

    public class Handler(
        IUserAccessor userAccessor, 
        AppDbContext dbContext, 
        IMapper mapper,
        IPhotoService photoService) : IRequestHandler<Command, Result<PhotoResponse>>
    {
        public async Task<Result<PhotoResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var uploadResult = await photoService.UploadPhoto(request.File);

            if (uploadResult == null)
            {
                return Result<PhotoResponse>.Failure("Failed to upload photo", 400);
            }
            
            var user = await userAccessor.GetUserAsync();

            var photo = new Photo
            {
                Url = uploadResult.Url,
                PublicId = uploadResult.PublicId,
                UserId = user.Id,
            };
            
            user.ImageUrl ??= uploadResult.Url;
            
            await dbContext.Photos.AddAsync(photo, cancellationToken);
            
            var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;

            return result
                ? Result<PhotoResponse>.Success(mapper.Map<PhotoResponse>(photo))
                : Result<PhotoResponse>.Failure("Problem saving photo to DB", 400);
        }
    }
}