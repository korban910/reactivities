using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configurations;

public class UserFollowingConfiguration : IEntityTypeConfiguration<UserFollowing>
{
    public void Configure(EntityTypeBuilder<UserFollowing> builder)
    {
        CreateUserFollowingTable(builder);
    }

    private static void CreateUserFollowingTable(EntityTypeBuilder<UserFollowing> builder)
    {
        builder.ToTable("UserFollowings");

        builder.HasKey(u => new
        {
            u.ObserverId,
            u.TargetId
        });
        
        builder.HasOne(u => u.Observer)
            .WithMany(u => u.Followings)
            .HasForeignKey(u => u.ObserverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Target)
            .WithMany(u => u.Followers)
            .HasForeignKey(u => u.TargetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}