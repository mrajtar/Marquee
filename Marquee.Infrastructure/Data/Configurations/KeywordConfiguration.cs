using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
{
    public void Configure(EntityTypeBuilder<Keyword> builder)
    {
        builder.HasKey(k => k.Id);
        
        builder.Property(k => k.Name)
            .IsRequired()
            .HasMaxLength(250);
        
        builder.HasIndex(k => k.TmdbId)
            .IsUnique()
            .HasFilter("[TmdbId] IS NOT NULL");
    }
}