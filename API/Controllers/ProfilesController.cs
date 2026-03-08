using Application.Profiles.Commands;
using Application.Profiles.Queries;
using Contracts.Profile.Requests;
using Contracts.Profile.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfilesController : BaseApiController
{
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoResponse>> AddPhoto(AddPhotoRequest request)
    {
        var result = await Mediator.Send(new AddPhoto.Command
        {
            File = request.File
        });
        
        return HandleResult(result);
    }

    [HttpGet("{userId}/photos")]
    public async Task<ActionResult<List<PhotoResponse>>> GetPhotos(string userId)
    {
        var result = await Mediator.Send(new GetProfilePhotos.Query
        {
            UserId = userId
        });
        
        return HandleResult(result);
    }
    
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserProfileResponse>> GetProfile(string userId)
    {
        var result = await Mediator.Send(new GetProfile.Query
        {
            UserId = userId
        });
        
        return HandleResult(result);
    }

    [HttpPut("{photoId}/set-main-photo")]
    public async Task<ActionResult> SetMainPhoto(string photoId)
    {
        var result = await Mediator.Send(new SetMainPhoto.Command
        {
            PhotoId = photoId
        });

        return HandleResult(result);
    }

    [HttpDelete("{photoId}/photos")]
    public async Task<ActionResult> DeletePhoto(string photoId)
    {
        var result = await Mediator.Send(new DeletePhoto.Command
        {
            PhotoId = photoId
        });
        
        return HandleResult(result);
    }

    [HttpPut("edit-profile")]
    public async Task<ActionResult> EditProfile(EditProfileRequest request)
    {
        var result = await Mediator.Send(new EditProfile.Command
        {
            DisplayName = request.DisplayName,
            Bio = request.Bio,
        });
        
        return HandleResult(result);
    }

    [HttpPost("{userId}/follow")]
    public async Task<ActionResult> FollowToggle(string userId)
    {
        var result = await Mediator.Send(new FollowToggle.Command
        {
            TargetUserId = userId
        });

        return HandleResult(result);
    }

    [HttpGet("{userId}/follow-list")]
    public async Task<ActionResult> GetFollowings(string userId, string predicate)
    {
        var result = await Mediator.Send(new GetFollowings.Query
        {
            UserId = userId,
            Predicate = predicate
        });
        
        return  HandleResult(result);
    }

    [HttpGet("{userId}/activities")]
    public async Task<ActionResult<List<UserActivityResponse>>> GetActivities(string userId, string filter)
    {
        var result = await Mediator.Send(new GetUserActivities.Query
        {
            UserId = userId,
            Filter = filter
        });

        return HandleResult(result);
    }
}