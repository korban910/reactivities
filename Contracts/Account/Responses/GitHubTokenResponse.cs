using System.Text.Json.Serialization;

namespace Contracts.Account.Responses;

public class GitHubTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
}