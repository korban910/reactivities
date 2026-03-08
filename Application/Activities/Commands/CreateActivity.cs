using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Contracts.Activity.Requests;
using Domain;
using MediatR;
using Persistence.Contexts;

namespace Application.Activities.Commands;

public class CreateActivity
{
    public class Command : IRequest<Result<string>>
    {
        public required CreateActivityRequest Activity { get; set; }
    }

    public class Handler(
        AppDbContext context, 
        IMapper mapper,
        IUserAccessor userAccessor
       // IValidator<Command> validator  <== ValidationBehavior is doing the work
        ) : IRequestHandler<Command, Result<string>>
    {
        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
          //  await validator.ValidateAndThrowAsync(request, cancellationToken); <== ValidationBehavior is doing the work
            var user = await userAccessor.GetUserAsync();
          
            var activity = mapper.Map<Activity>(request.Activity);
            
            await context.Activities.AddAsync(activity, cancellationToken);

            var attendee = new ActivityAttendee()
            {
                ActivityId = activity.Id,
                UserId = user.Id,
                IsHost = true,
            };
            
            await context.ActivityAttendees.AddAsync(attendee, cancellationToken);
            
            var saved = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!saved)
            {
                return Result<string>.Failure("Failed to save activity", 400);
            }
            
            return Result<string>.Success(activity.Id);
        }
    }
}