using Contracts.Profile.Responses;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IPhotoService
{
    Task<PhotoUploadResponse?> UploadPhoto(IFormFile file);
    Task<string> DeletePhoto(string publicId);
}