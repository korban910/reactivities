using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configurations;

public class ActivityAttendeeConfiguration : IEntityTypeConfiguration<ActivityAttendee>
{
    public void Configure(EntityTypeBuilder<ActivityAttendee> builder)
    {
        CreateActivityAttendeeTable(builder);
    }

    private static void CreateActivityAttendeeTable(EntityTypeBuilder<ActivityAttendee> builder)
    {
        builder.ToTable("ActivityAttendees");
        
        builder.HasKey(a => new { a.ActivityId, a.UserId });
        
        builder.HasOne(a => a.Activity)
            .WithMany(a => a.Attendees)
            .HasForeignKey(a => a.ActivityId);
        
        builder.HasOne(a => a.User)
            .WithMany(u => u.Activities)
            .HasForeignKey(a => a.UserId);
    }
}