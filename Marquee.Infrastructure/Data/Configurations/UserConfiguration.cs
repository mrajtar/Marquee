using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marquee.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Username)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(r => r.Username)
            .IsUnique();
        builder.Property(r => r.DisplayName)
            .HasMaxLength(50);
        builder.Property(r => r.Bio)
            .HasMaxLength(1000);
        builder.Property(r => r.ProfileImageUrl)
            .HasMaxLength(500);
    }
}