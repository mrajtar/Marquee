using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(2);
        builder.HasIndex(c => c.Code)
            .IsUnique();
        builder.HasIndex(c => c.TmdbId)
            .IsUnique()
            .HasFilter("[TmdbId] IS NOT NULL");
    }
}