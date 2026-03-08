namespace Contracts.Profile.Requests;

public class EditProfileRequest
{
    public required string DisplayName { get; set; }
    public string? Bio { get; set; }
}