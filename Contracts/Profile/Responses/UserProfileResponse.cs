namespace Contracts.Profile.Responses;

public class UserProfileResponse
{
    public required string Id { get; set; }
    public required string DisplayName { get; set; }
    public string? Bio  { get; set; }
    public string? ImageUrl { get; set; }
    public bool Following { get; set; } = false;
    public int FollowersCount { get; set; } = 0;
    public int FollowingCount { get; set; } = 0;
}