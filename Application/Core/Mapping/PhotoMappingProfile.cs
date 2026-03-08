using AutoMapper;
using Contracts.Profile.Responses;
using Domain;

namespace Application.Core.Mapping;

public class PhotoMappingProfile : Profile
{
    public PhotoMappingProfile()
    {
        CreateMap<Photo, PhotoUploadResponse>();
        CreateMap<PhotoUploadResponse, Photo>();
        CreateMap<Photo, PhotoResponse>();
    }
}