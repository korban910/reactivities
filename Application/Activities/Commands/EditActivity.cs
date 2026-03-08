using Application.Core;
using AutoMapper;
using Contracts.Activity.Requests;
using MediatR;
using Persistence.Contexts;

namespace Application.Activities.Commands;

public class EditActivity
{
    public class Command : IRequest<Result<Unit>>
    {
        public required EditActivityRequest Activity { get; set; }
    }
    
    public class Handler(AppDbContext context, IMapper mapper ) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities.FindAsync([request.Activity.Id], cancellationToken);
            
            if (activity == null)
            {
                return Result<Unit>.Failure("Activity not found", 404);
            }
            
            mapper.Map(request.Activity, activity);
            
            var saved = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!saved)
            {
                return Result<Unit>.Failure("Failed to edit activity", 400);
            }
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}