using Microsoft.AspNetCore.Http;

namespace Contracts.Profile.Requests;

public class AddPhotoRequest
{
    public required IFormFile File {get; set;}
}