using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        CreateUserTable(builder);
    }

    private static void CreateUserTable(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(u => u.DisplayName);
        
        builder.Property(u => u.Bio);
        
        builder.Property(u => u.ImageUrl);
        
        builder.HasMany(u => u.Activities)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);
        
        builder.HasMany(u => u.Photos)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);
        
        builder.HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);
    }
}