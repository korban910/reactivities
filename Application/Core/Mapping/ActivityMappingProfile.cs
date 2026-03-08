using AutoMapper;
using Contracts.Activity.Requests;
using Contracts.Activity.Responses;
using Contracts.Profile.Responses;
using Domain;

namespace Application.Core.Mapping;

public class ActivityMappingProfile : Profile
{
    public ActivityMappingProfile()
    {
        CreateMap<Activity, Activity>();

        CreateMap<CreateActivityRequest, Activity>();

        CreateMap<EditActivityRequest, Activity>();

        CreateMap<Activity, ActivityResponse>()
            .ForMember(d => d.HostDisplayName,
                o => o.MapFrom(s =>
                    s.Attendees.FirstOrDefault(a => a.IsHost)!.User!.DisplayName))
            .ForMember(d => d.HostId,
                o => o.MapFrom(s =>
                    s.Attendees.FirstOrDefault(a => a.IsHost)!.User!.Id))
            .ForMember(d => d.City, o => o.MapFrom(s => 
                s.Location.City))
            .ForMember(d => d.Venue, o => o.MapFrom(s => 
                s.Location.Venue))
            .ForMember(d => d.Longitude, o => o.MapFrom(s => 
                s.Location.Longitude))
            .ForMember(d => d.Latitude, o => o.MapFrom(s => 
                s.Location.Latitude));

        CreateMap<Activity, UserActivityResponse>()
            .ForMember(d => d.Id, o =>
                o.MapFrom(s => s.Id))
            .ForMember(d => d.Title, o =>
                o.MapFrom(s => s.Title))
            .ForMember(d => d.Category, o =>
                o.MapFrom(s => s.Category))
            .ForMember(d => d.Date, o => 
                o.MapFrom(s => s.Date));

    }
}