using Application.Comments.Commands;
using Application.Comments.Queries;
using Contracts.Comment.Requests;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

public class CommentHub(IMediator mediator) : Hub
{
    public async Task SendComment(CommentRequest request)
    {
        var comment = await mediator.Send(new AddComment.Command()
        {
            ActivityId = request.ActivityId,
            Body = request.Body,
        });
        
        await Clients.Group(request.ActivityId).SendAsync(Environment.GetEnvironmentVariable("SIGNALR_SEND_METHOD_NAME")!, comment.Value);
    }
    
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var activityId = httpContext?.Request.Query["activityId"];

        if (string.IsNullOrEmpty(activityId))
        {
            throw new HubException("No activity with this id");
        }
        
        await Groups.AddToGroupAsync(Context.ConnectionId, activityId!);

        var result = await mediator.Send(new GetCommentList.Query()
        {
            ActivityId = activityId!
        });
        
        await Clients.Caller.SendAsync(Environment.GetEnvironmentVariable("SIGNALR_CONNECTION_METHOD_NAME")!, result.Value); 
    }
}