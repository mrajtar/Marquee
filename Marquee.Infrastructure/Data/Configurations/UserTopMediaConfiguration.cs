using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class UserTopMediaConfiguration : IEntityTypeConfiguration<UserTopMedia>
{
    public void Configure(EntityTypeBuilder<UserTopMedia> builder)
    {
        builder.HasKey(utm => new { utm.UserId, utm.Position });

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_UserTopMedia_Position",
            "[Position] >= 1 AND [Position] <= 5"));
        
        builder.Property(utm => utm.Note)
            .HasMaxLength(120);
        
        builder.HasIndex(utm => new { utm.UserId, utm.MediaId })
            .IsUnique();
        builder.HasOne(utm => utm.User)
            .WithMany(utm => utm.TopMedia)
            .HasForeignKey(utm => utm.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(utm => utm.Media)
            .WithMany()
            .HasForeignKey(utm => utm.MediaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}