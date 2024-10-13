using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = CommentarySystem.Server.Data.Entities.File;

namespace WebApplication1.Data.DataConfiguration;

public class FileConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.HasKey(f => f.FileId);

        builder.Property(f => f.FileId)
            .ValueGeneratedOnAdd();


        // Поле FileType обов'язкове
        builder.Property(f => f.FileType)
            .IsRequired()
            .HasMaxLength(10); // Наприклад, тип: "image", "text"

        // Поле FileSize обов'язкове
        builder.Property(f => f.FileSize)
            .IsRequired();

        // Зв'язок з коментарем
        builder.HasOne(f => f.Comment)
            .WithMany(c => c.Files)
            .HasForeignKey(f => f.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}