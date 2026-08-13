using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class MediaInteractionConfiguration : IEntityTypeConfiguration<MediaInteraction>
{
    public void Configure(EntityTypeBuilder<MediaInteraction> builder)
    {
        builder.HasKey(mi => mi.Id);

        builder.HasOne(mi => mi.User)
            .WithMany(u => u.MediaInteractions)
            .HasForeignKey(mi => mi.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(mi => mi.Media)
            .WithMany(m => m.Interactions)
            .HasForeignKey(mi => mi.MediaId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(mi => new { mi.UserId, mi.MediaId, mi.CreatedAt });
    }
}