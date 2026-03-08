using AutoMapper;
using Contracts.Comment.Responses;
using Domain;

namespace Application.Core.Mapping;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CommentResponse>()
            .ForMember(d => d.UserId, o => o.MapFrom(
                s => s.User.Id))
            .ForMember(d => d.DisplayName, o => o.MapFrom(
                s => s.User.DisplayName))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s =>
                s.User.ImageUrl));
    }
}