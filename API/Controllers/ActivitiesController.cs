using Application.Activities.Commands;
using Application.Activities.Queries;
using Application.Core;
using Contracts.Activity.Requests;
using Contracts.Activity.Responses;
using Domain;
using Infrastructure.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<ActivityResponse, DateTimeOffset?>>> GetActivities([FromQuery]ActivityParamsRequest activityParamsRequest)
    {
        var result = await Mediator.Send(new GetActivityList.Query
        {
            Params = activityParamsRequest
        });

        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityResponse>> GetActivity(string id)
    {
        var result = await Mediator.Send(new GetActivityDetails.Query
        {
            Id = id
        });

        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateActivity(CreateActivityRequest activity)
    {
        var result = await Mediator.Send(new CreateActivity.Command
        {
            Activity = activity
        });

        return HandleResult(result);
    }

    [HttpPut("{activityId}")]
    [Authorize(Policy = PolicyName.IsActivityHost)]
    public async Task<ActionResult> EditActivity(string activityId, EditActivityRequest activity)
    {
        activity.Id = activityId;
        var result = await Mediator.Send(new EditActivity.Command
        {
            Activity = activity
        }
        );

        return HandleResult(result);
    }

    [HttpDelete("{activityId}")]
    [Authorize(Policy = PolicyName.IsActivityHost)]
    public async Task<ActionResult> DeleteActivity(string activityId)
    {
        var result = await Mediator.Send(new DeleteActivity.Command
        {
            Id = activityId
        });

        return HandleResult(result);
    }

    [HttpPost("{activityId}/attend")]
    public async Task<ActionResult> Attend(string activityId)
    {
        var result = await Mediator.Send(new UpdateAttendance.Command()
        {
            ActivityId = activityId
        });

        return HandleResult(result);
    }
}
