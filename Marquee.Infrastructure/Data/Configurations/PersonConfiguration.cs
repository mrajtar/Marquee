using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(p => p.Biography)
            .HasMaxLength(5000);
        builder.Property(p => p.ProfileImageUrl)
            .HasMaxLength(500);
        builder.Property(p => p.PlaceOfBirth)
            .HasMaxLength(500);
        
        builder.HasIndex(p => p.TmdbId)
            .IsUnique()
            .HasFilter("[TmdbId] IS NOT NULL");
    }
}