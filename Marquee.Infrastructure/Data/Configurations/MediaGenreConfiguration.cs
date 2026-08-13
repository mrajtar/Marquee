using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class MediaGenreConfiguration : IEntityTypeConfiguration<MediaGenre>
{
    public void Configure(EntityTypeBuilder<MediaGenre> builder)
    {
        builder.HasKey(mg => new { mg.MediaId, mg.GenreId });
        
        builder.HasOne(mg => mg.Media)
            .WithMany(m => m.MediaGenres)
            .HasForeignKey(mg => mg.MediaId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(mg => mg.Genre)
            .WithMany(g => g.MediaGenres)
            .HasForeignKey(mg => mg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}