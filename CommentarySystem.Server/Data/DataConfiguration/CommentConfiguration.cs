using CommentarySystem.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Data.DataConfiguration;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{

    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.CommentId);

        builder.Property(c => c.CommentId)
            .ValueGeneratedOnAdd();

        // Поле Text обов'язкове
        builder.Property(c => c.Text)
            .IsRequired();

        // Поле CreatedAt обов'язкове
        builder.Property(c => c.CreatedAt)
            .IsRequired();

        // Зв'язок з користувачем
        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Зв'язок з батьківськими коментарями (каскад)
        builder.HasOne(c => c.ParentComment)
            .WithMany(c => c.ChildComments)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}