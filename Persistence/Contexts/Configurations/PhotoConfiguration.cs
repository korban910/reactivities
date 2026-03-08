using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        CreatePhotoTable(builder);
    }

    private static void CreatePhotoTable(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable("Photos");
        
        builder.HasKey(l => l.Id);
        
        builder.Property(l => l.Url)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(l => l.PublicId).IsRequired();
        
        builder.Property(l => l.UserId).IsRequired();
        
        builder.HasOne(l => l.User)
            .WithMany(u => u.Photos)
            .HasForeignKey(l => l.UserId);
    }
}