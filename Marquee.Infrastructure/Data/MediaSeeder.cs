using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;
using Marquee.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Marquee.Infrastructure.Data;

public class MediaSeeder
{
    private readonly ITmdbService _tmdbService;
    private readonly MarqueeDbContext _dbContext;

    public MediaSeeder(ITmdbService tmdbService, MarqueeDbContext dbContext)
    {
        _tmdbService = tmdbService;
        _dbContext = dbContext;
    }

    public async Task ImportPopularMoviesAsync(int targetCount = 500, CancellationToken cancellationToken = default)
    {
        int page = 1;
        int importedCount = 0;

        while (importedCount < targetCount && page <= 25)
        {
            var movies = await _tmdbService.GetPopularMoviesAsync(page, cancellationToken);
            if (movies.Count == 0) break;

            foreach (var tmdbMovie in movies)
            {
                if (importedCount >= targetCount) break;

                if (await _dbContext.Media.AnyAsync(m => m.TmdbId == tmdbMovie.Id, cancellationToken))
                    continue;

                var movie = new Movie
                {
                    Title = tmdbMovie.Title,
                    OriginalTitle = tmdbMovie.OriginalTitle,
                    Overview = tmdbMovie.Overview,
                    PosterUrl = tmdbMovie.PosterPath != null
                        ? $"https://image.tmdb.org/t/p/w500{tmdbMovie.PosterPath}"
                        : null,
                    BackdropUrl = tmdbMovie.BackdropPath != null
                        ? $"https://image.tmdb.org/t/p/w1280{tmdbMovie.BackdropPath}"
                        : null,
                    ReleaseDate = ParseDate(tmdbMovie.ReleaseDate),
                    RuntimeMinutes = tmdbMovie.Runtime,
                    TmdbId = tmdbMovie.Id,
                    ImdbId = tmdbMovie.ImdbId,
                    Status = MapStatus(tmdbMovie.Status)
                };

                await AddGenresAsync(movie, tmdbMovie.GenreIds, cancellationToken);
                await AddCountriesAsync(movie, tmdbMovie.OriginCountry, cancellationToken);

                _dbContext.Media.Add(movie);
                importedCount++;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            page++;
            await Task.Delay(200, cancellationToken);
        }
    }

    public async Task ImportPopularTvShowsAsync(int targetCount = 500, CancellationToken cancellationToken = default)
    {
        int page = 1;
        int importedCount = 0;

        while (importedCount < targetCount && page <= 25)
        {
            var tvShows = await _tmdbService.GetPopularTvShowsAsync(page, cancellationToken);
            if (tvShows.Count == 0) break;

            foreach (var tmdbShow in tvShows)
            {
                if (importedCount >= targetCount) break;

                if (await _dbContext.Media.AnyAsync(m => m.TmdbId == tmdbShow.Id, cancellationToken))
                    continue;

                var tvShow = new TvShow
                {
                    Title = tmdbShow.Name ?? tmdbShow.Title,
                    OriginalTitle = tmdbShow.OriginalName ?? tmdbShow.OriginalTitle,
                    Overview = tmdbShow.Overview,
                    PosterUrl = tmdbShow.PosterPath != null
                        ? $"https://image.tmdb.org/t/p/w500{tmdbShow.PosterPath}"
                        : null,
                    BackdropUrl = tmdbShow.BackdropPath != null
                        ? $"https://image.tmdb.org/t/p/w1280{tmdbShow.BackdropPath}"
                        : null,
                    ReleaseDate = ParseDate(tmdbShow.FirstAirDate),
                    TmdbId = tmdbShow.Id,
                    Status = MapStatus(tmdbShow.Status),
                    NumberOfSeasons = tmdbShow.NumberOfSeasons,
                    NumberOfEpisodes = tmdbShow.NumberOfEpisodes
                };

                await AddGenresAsync(tvShow, tmdbShow.GenreIds, cancellationToken);
                await AddCountriesAsync(tvShow, tmdbShow.OriginCountry, cancellationToken);

                _dbContext.Media.Add(tvShow);
                importedCount++;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            page++;
            await Task.Delay(200, cancellationToken);
        }
    }
    
    private async Task AddGenresAsync(Media media, List<int> genreIds, CancellationToken ct)
    {
        foreach (var genreId in genreIds)
        {
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.TmdbId == genreId, ct);
            if (genre != null)
            {
                media.MediaGenres.Add(new MediaGenre { Genre = genre, Media = media });
            }
        }
    }

    private async Task AddCountriesAsync(Media media, List<string> countryCodes, CancellationToken ct)
    {
        foreach (var code in countryCodes)
        {
            var country = await _dbContext.Countries.FirstOrDefaultAsync(c => c.Code == code, ct);
            if (country != null)
            {
                media.MediaCountries.Add(new MediaCountry { Country = country, Media = media });
            }
        }
    }
    
    private DateTime? ParseDate(string? dateStr) =>
        DateTime.TryParse(dateStr, out var date) ? date : null;

    private Status? MapStatus(string? status) => status switch
    {
        "Released" => Status.Released,
        "Planned" => Status.Planned,
        "In Production" => Status.InProduction,
        "Ended" => Status.Ended,
        "Returning Series" => Status.Ongoing,
        "Cancelled" => Status.Cancelled,
        _ => null
    };
}