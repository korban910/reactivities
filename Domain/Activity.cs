using System;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class Activity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Title { get; set; }
    public DateTimeOffset Date { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public bool IsCancelled { get; set; }
    
    // location props
    public Location Location { get; set; }
    
    // navigation properties
    public ICollection<ActivityAttendee> Attendees { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
}