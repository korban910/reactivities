using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configurations;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        CreateActivityTable(builder);
    }

    private static void CreateActivityTable(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("Activities");

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.Date);

        builder.Property(a => a.Title).IsRequired();

        builder.Property(a => a.Date);

        builder.Property(a => a.Description).IsRequired();

        builder.Property(a => a.Category).IsRequired();

        builder.Property(a => a.IsCancelled);

        builder.OwnsOne(a => a.Location, add =>
        {
            add.Property(l => l.City).HasColumnName("City");
            add.Property(l => l.Venue).HasColumnName("Venue");
            add.Property(l => l.Latitude).HasColumnName("Latitude");
            add.Property(l => l.Longitude).HasColumnName("Longitude");
        });
        
        builder.Navigation(a => a.Location).IsRequired();

        builder.HasMany(a => a.Attendees)
            .WithOne(at => at.Activity)
            .HasForeignKey(a => a.ActivityId);
        
        builder.HasMany(a => a.Comments)
            .WithOne(c => c.Activity)
            .HasForeignKey(a => a.ActivityId);
    }
}