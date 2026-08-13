using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class MediaListItemConfiguration : IEntityTypeConfiguration<MediaListItem>
{
    public void Configure(EntityTypeBuilder<MediaListItem> builder)
    {
        builder.HasKey(mli => new { mli.MediaListId, mli.MediaId });
        
        builder.HasOne(mli => mli.MediaList)
            .WithMany(mli => mli.Items)
            .HasForeignKey(mli => mli.MediaListId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(mli => mli.Media)
            .WithMany()
            .HasForeignKey(mli => mli.MediaId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(mli => new { mli.MediaListId, mli.AddedAt });
            
    }
}