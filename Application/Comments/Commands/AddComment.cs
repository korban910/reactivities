using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Contracts.Comment.Responses;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Comments.Commands;

public class AddComment
{
    public class Command : IRequest<Result<CommentResponse>>
    {
        public required string Body { get; set; }
        public required string ActivityId { get; set; }
    }
    
    public class Handler(
        AppDbContext dbContext, 
        IUserAccessor userAccessor,
        IMapper mapper): IRequestHandler<Command, Result<CommentResponse>>
    {
        public async Task<Result<CommentResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await dbContext.Activities
                .Include(a => a.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(a => a.Id == request.ActivityId, cancellationToken);

            if (activity == null)
            {
                return Result<CommentResponse>.Failure("Could not find activity", 404);
            }
            
            if (await userAccessor.GetUserAsync() is not { } user)
            {
                return Result<CommentResponse>.Failure("Could not find user", 404);
            }
            
            var comment = new Comment
            {
                Body = request.Body,
                ActivityId = activity.Id,
                UserId = user.Id
            };
            
            await dbContext.Comments.AddAsync(comment, cancellationToken);

            var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;

            return result
                ? Result<CommentResponse>.Success(mapper.Map<CommentResponse>(comment))
                : Result<CommentResponse>.Failure("Could not add comment", 400);
        }
    }
}