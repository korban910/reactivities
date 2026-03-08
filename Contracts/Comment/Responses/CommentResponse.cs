namespace Contracts.Comment.Responses;

public class CommentResponse
{
    public required string Id { get; set; }
    public required string Body { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public required string UserId { get; set; }
    public required string DisplayName { get; set; }
    public string? ImageUrl { get; set; }
}