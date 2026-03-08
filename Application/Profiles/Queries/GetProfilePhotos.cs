using Application.Core;
using AutoMapper;
using Contracts.Profile.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Profiles.Queries;

public class GetProfilePhotos
{
    public class Query : IRequest<Result<List<PhotoResponse>>>
    {
        public required string UserId { get; set; }
    }

    public class Handler(AppDbContext dbContext, IMapper mapper) : IRequestHandler<Query, Result<List<PhotoResponse>>>
    {
        public async Task<Result<List<PhotoResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var photos = await dbContext.Users
                .Where(u => u.Id == request.UserId)
                .SelectMany(u => u.Photos)
                .ToListAsync(cancellationToken);
            
            return Result<List<PhotoResponse>>.Success(mapper.Map<List<PhotoResponse>>(photos));
        }
    }
}