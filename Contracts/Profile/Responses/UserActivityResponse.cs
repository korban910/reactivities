namespace Contracts.Profile.Responses;

public class UserActivityResponse
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string Category { get; set; }
    public required DateTimeOffset Date { get; set; }
}