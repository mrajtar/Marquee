using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.HasKey(r => new { r.UserId, r.MediaId });

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Rating_Value",
            "[Value] >= 1 AND [Value] <= 20"));
        
        builder.HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(r => r.Media)
            .WithMany(m => m.Ratings)
            .HasForeignKey(r => r.MediaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}