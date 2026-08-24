using Marquee.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;

namespace Marquee.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(r => r.DisplayName)
            .HasMaxLength(50);
        builder.Property(r => r.Bio)
            .HasMaxLength(1000);
        builder.Property(r => r.ProfileImageUrl)
            .HasMaxLength(500);
        builder.Property(u => u.CreatedAt)
            .IsRequired();
    }
}