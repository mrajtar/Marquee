using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class MediaPersonConfiguration : IEntityTypeConfiguration<MediaPerson>
{
    public void Configure(EntityTypeBuilder<MediaPerson> builder)
    {
        builder.HasKey(mp => new { mp.MediaId, mp.PersonId, mp.Role });

        builder.Property(mp => mp.CharacterName)
            .HasMaxLength(200);
        
        builder.HasOne(mp => mp.Person)
            .WithMany(m => m.MediaPeople)
            .HasForeignKey(mp => mp.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(mp => mp.Media)
            .WithMany(m => m.MediaPeople)
            .HasForeignKey(mp => mp.MediaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}