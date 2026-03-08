namespace Contracts.Comment.Requests;

public class CommentRequest
{
    public required string Body { get; set; }
    public required string ActivityId  { get; set; }
}