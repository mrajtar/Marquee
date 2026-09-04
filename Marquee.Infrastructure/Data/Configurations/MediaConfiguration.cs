using Marquee.Domain.Entities;
using Marquee.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.HasIndex(m => m.TrendingScore);
        
        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(m => m.OriginalTitle)
            .HasMaxLength(200);
        builder.Property(m => m.PosterUrl)
            .HasMaxLength(500);
        builder.Property(m => m.BackdropUrl)
            .HasMaxLength(500);
        
        builder.HasDiscriminator(m => m.Type)
            .HasValue<Movie>(MediaType.Movie)
            .HasValue<TvShow>(MediaType.TvShow);

        builder.HasIndex(m => m.Title);
        builder.HasIndex(m => m.ReleaseDate);
        builder.HasIndex(m => m.TmdbId)
            .IsUnique()
            .HasFilter("[TmdbId] IS NOT NULL");
        builder.HasIndex(m => m.ImdbId)
            .IsUnique()
            .HasFilter("[ImdbId] IS NOT NULL");
    }
}