using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Content)
            .IsRequired();
        
        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(r => r.Media)
            .WithMany(m => m.Reviews)
            .HasForeignKey(r => r.MediaId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(r => new { r.MediaId, r.CreatedAt });
    }
}