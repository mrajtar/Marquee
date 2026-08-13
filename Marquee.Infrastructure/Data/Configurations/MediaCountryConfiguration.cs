using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class MediaCountryConfiguration : IEntityTypeConfiguration<MediaCountry>
{
    public void Configure(EntityTypeBuilder<MediaCountry> builder)
    {
        builder.HasKey(mc => new { mc.MediaId, mc.CountryId });
        
        builder.HasOne(mc => mc.Media)
            .WithMany(m => m.MediaCountries)
            .HasForeignKey(mc => mc.MediaId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(mc => mc.Country)
            .WithMany(c => c.MediaCountries)
            .HasForeignKey(mc => mc.CountryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}