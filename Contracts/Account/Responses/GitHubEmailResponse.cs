namespace Contracts.Account.Responses;

public class GitHubEmailResponse
{
    public string Email { get; set; } = string.Empty;
    public bool Primary { get; set; }
    public bool Verified { get; set; }
}