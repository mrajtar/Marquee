using System.Text.Json.Serialization;
using Marquee.Application.DTOs.Media;

namespace Marquee.Application.Interfaces.Services;

public interface ITmdbService
{
    Task<List<TmdbGenre>> GetGenresAsync(CancellationToken cancellationToken);
    Task<List<TmdbCountry>> GetCountriesAsync(CancellationToken cancellationToken);
    Task<List<TmdbMediaDto>> GetPopularMoviesAsync(int page = 1, CancellationToken cancellationToken = default);
    Task<List<TmdbMediaDto>> GetPopularTvShowsAsync(int page = 1, CancellationToken cancellationToken = default);
    Task<TmdbMediaDto> GetMovieDetailsAsync(int tmdbId, CancellationToken cancellationToken = default);
    Task<TmdbMediaDto> GetTvShowDetailsAsync(int tmdbId, CancellationToken cancellationToken = default);
}

public class TmdbGenre
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class TmdbCountry
{
    [JsonPropertyName("iso_3166_1")]
    public string Code { get; set; } = null!;
    [JsonPropertyName("english_name")]
    public string Name { get; set; } = null!;
}