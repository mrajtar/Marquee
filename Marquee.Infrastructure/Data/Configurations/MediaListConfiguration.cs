using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class MediaListConfiguration : IEntityTypeConfiguration<MediaList>
{
    public void Configure(EntityTypeBuilder<MediaList> builder)
    {
        builder.HasKey(ml => ml.Id);
        
        builder.Property(ml => ml.Name)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(ml => ml.Description)
            .HasMaxLength(600);
        builder.HasOne(ml => ml.User)
            .WithMany(u => u.MediaLists)
            .HasForeignKey(ml => ml.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ml => new { ml.UserId, ml.Name });
    }
}