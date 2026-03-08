using AutoMapper;
using Contracts.Profile.Responses;
using Domain;

namespace Application.Core.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        string? currentUserId = null;
        CreateMap<User, UserProfileResponse>()
            .ForMember(d => d.Id, o => o
                .MapFrom(s => s.Id))
            .ForMember(d => d.DisplayName, o => 
                o.MapFrom(s => s.DisplayName))
            .ForMember(d => d.ImageUrl, o => 
                o.MapFrom(s => s.ImageUrl))
            .ForMember(d => d.Bio, o => 
                o.MapFrom(s => s.Bio))
            .ForMember(d => d.FollowersCount, o => 
                o.MapFrom(s => s.Followers.Count))
            .ForMember(d => d.FollowingCount, o => 
                o.MapFrom(s => s.Followings.Count))
            .ForMember(d => d.Following, o => 
                o.MapFrom(s => s.Followers.Any(x => x.Observer.Id == currentUserId)));
        
        CreateMap<ActivityAttendee, UserProfileResponse>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User!.DisplayName))
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.User!.Bio))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User!.ImageUrl))
            .ForMember(d => d.Id, o => o.MapFrom(s => s.User!.Id))
            .ForMember(d => d.FollowersCount, o => 
                o.MapFrom(s => s.User.Followers.Count))
            .ForMember(d => d.FollowingCount, o => 
                o.MapFrom(s => s.User.Followings.Count))
            .ForMember(d => d.Following, o => 
                o.MapFrom(s => s.User.Followers.Any(x => x.Observer.Id == currentUserId)));
    }
}