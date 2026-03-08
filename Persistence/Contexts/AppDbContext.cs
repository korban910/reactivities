using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<UserFollowing> UserFollowings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        
        /*builder.Entity<ActivityAttendee>(x => x.HasKey(a => new { a.ActivityId, a.UserId }));
        
        builder.Entity<ActivityAttendee>()
            .HasOne(a => a.User)
            .WithMany(u => u.Activities)
            .HasForeignKey(a => a.UserId);
        
        builder.Entity<ActivityAttendee>()
            .HasOne(a => a.Activity)
            .WithMany(a => a.Attendees)
            .HasForeignKey(a => a.ActivityId);*/
        
        // without any migrations, this would do the trick 
        // if Database use DateTime
        /* var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
            }
        } */
    }
}