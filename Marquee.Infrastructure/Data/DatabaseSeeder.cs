using Marquee.Domain.Entities;
using Marquee.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Marquee.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedReferenceDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MarqueeDbContext>();
        var tmdbService = scope.ServiceProvider.GetRequiredService<ITmdbService>();
        
        await SeedGenresAsync(dbContext, tmdbService);
        await SeedCountriesAsync(dbContext, tmdbService);
    }

    private static async Task SeedGenresAsync(MarqueeDbContext dbContext, ITmdbService tmdbService)
    {
        if (await dbContext.Genres.AnyAsync())
            return;

        var tmdbGenres = await tmdbService.GetGenresAsync(CancellationToken.None);
        var genres = tmdbGenres.Select(g => new Genre
        {
            Name = g.Name,
            TmdbId = g.Id
        }).ToList();

        dbContext.Genres.AddRange(genres);
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedCountriesAsync(MarqueeDbContext dbContext, ITmdbService tmdbService)
    {
        if (await dbContext.Countries.AnyAsync())
            return;

        var tmdbCountries = await tmdbService.GetCountriesAsync(CancellationToken.None);
        var countries = tmdbCountries.Select(c => new Country
        {
            Name = c.Name,
            Code = c.Code,
            TmdbId = null
        }).ToList();

        dbContext.Countries.AddRange(countries);
        await dbContext.SaveChangesAsync();
    }
}