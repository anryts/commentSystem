using CommentarySystem.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommentarySystem.Server.Data.DataConfiguration;

public class SelectedRangeConfiguration : IEntityTypeConfiguration<SelectedRange>
{
    public void Configure(EntityTypeBuilder<SelectedRange> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.StartIndex).IsRequired();
        builder.Property(x => x.EndIndex).IsRequired();
    }
}