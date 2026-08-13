using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(g => g.Id);
        
        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(g => g.Name)
            .IsUnique();

        builder.HasIndex(g => g.TmdbId)
            .IsUnique()
            .HasFilter("[TmdbId] IS NOT NULL");
    }
}