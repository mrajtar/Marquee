using Marquee.Application.Interfaces;
using Marquee.Domain.Entities;
using Marquee.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Marquee.Infrastructure.Data;

public class MarqueeDbContext : IdentityDbContext<User, IdentityRole<int>, int>, IUnitOfWork
{
    public MarqueeDbContext(DbContextOptions<MarqueeDbContext> options) : base(options) { }
    
    public DbSet<Media> Media => Set<Media>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<TvShow> TvShows => Set<TvShow>();
    public DbSet<Rating> Ratings => Set<Rating>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<UserTopMedia> UserTopMedia => Set<UserTopMedia>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Keyword> Keywords => Set<Keyword>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MarqueeDbContext).Assembly);
    }
}