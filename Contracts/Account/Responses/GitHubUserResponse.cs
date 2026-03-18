using System.Text.Json.Serialization;

namespace Contracts.Account.Responses;

public class GitHubUserResponse
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("avatar_url")]
    public string? ImageUrl { get; set; }
}