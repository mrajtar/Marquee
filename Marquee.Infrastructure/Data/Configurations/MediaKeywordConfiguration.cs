using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class MediaKeywordConfiguration : IEntityTypeConfiguration<MediaKeyword>
{
    public void Configure(EntityTypeBuilder<MediaKeyword> builder)
    {
        builder.HasKey(mk => new { mk.MediaId, mk.KeywordId });

        builder.HasOne(mk => mk.Media)
            .WithMany(k => k.MediaKeywords)
            .HasForeignKey(mk => mk.MediaId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(mk => mk.Keyword)
            .WithMany(k => k.MediaKeywords)
            .HasForeignKey(mk => mk.KeywordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}