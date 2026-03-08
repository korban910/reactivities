namespace Domain;

public class ActivityAttendee
{
    public string? UserId { get; set; }
    public User? User { get; set; }
    public string? ActivityId { get; set; }
    public Activity? Activity { get; set; }
    public bool IsHost { get; set; }
    public DateTimeOffset DateJoined { get; set; } = DateTimeOffset.UtcNow;
    
}