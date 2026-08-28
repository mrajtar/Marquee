using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class ReviewLikeConfiguration : IEntityTypeConfiguration<ReviewLike>
{
    public void Configure(EntityTypeBuilder<ReviewLike> builder)
    {
        builder.HasKey(x => new { x.UserId, x.ReviewId });
        builder.HasOne(x => x.User)
            .WithMany(u => u.ReviewLikes)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Review)
            .WithMany(r => r.Likes)
            .HasForeignKey(x => x.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}