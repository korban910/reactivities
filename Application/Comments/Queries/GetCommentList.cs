using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Comment.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Application.Comments.Queries;

public class GetCommentList
{
    public class Query : IRequest<Result<List<CommentResponse>>>
    {
        public required string ActivityId { get; set; }
    }
    
    public class Handler(AppDbContext dbContext, IMapper mapper): IRequestHandler<Query, Result<List<CommentResponse>>>
    {
        public async Task<Result<List<CommentResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var comments = await dbContext.Comments
                .Where(c => c.ActivityId == request.ActivityId)
                .OrderByDescending(c => c.CreatedAt)
                .ProjectTo<CommentResponse>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            
            return Result<List<CommentResponse>>.Success(comments);
        }
    }
}