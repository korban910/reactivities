using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        CreateCommentTable(builder);
    }

    private static void CreateCommentTable(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");
        
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Body).IsRequired();
        
        builder.Property(a => a.CreatedAt);
        
        builder.HasOne(a => a.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(a => a.UserId);
        
        builder.HasOne(a => a.Activity)
            .WithMany(a => a.Comments)
            .HasForeignKey(a => a.ActivityId);
    }
}