using System.Text.Json;
using Marquee.Application.DTOs.Media;
using Marquee.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Marquee.Application.Services;

public class TmdbService : ITmdbService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public TmdbService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Tmdb:ApiKey"] ?? throw new ArgumentNullException("Tmdb:ApiKey is missing");
    }

    public async Task<List<TmdbGenre>> GetGenresAsync(CancellationToken cancellationToken = default)
    {
        var movieGenres = await FetchAsync<TmdbGenreResponse>("genre/movie/list", cancellationToken);
        var tvGenres = await FetchAsync<TmdbGenreResponse>("genre/tv/list", cancellationToken);

       return movieGenres.Genres
            .Union(tvGenres.Genres, new TmdbGenreComparer())
            .ToList();
    }

    public async Task<List<TmdbCountry>> GetCountriesAsync(CancellationToken cancellationToken = default)
    {
        return await FetchAsync<List<TmdbCountry>>("configuration/countries", cancellationToken);
    }

    public async Task<List<TmdbMediaDto>> GetPopularMoviesAsync(int page = 1, CancellationToken cancellationToken = default)
    {
        var response = await FetchAsync<TmdbPagedResponse<TmdbMediaDto>>($"movie/popular?page={page}", cancellationToken);
        return response.Results;
    }
    
    public async Task<List<TmdbMediaDto>> GetPopularTvShowsAsync(int page = 1, CancellationToken cancellationToken = default)
    {
        var response = await FetchAsync<TmdbPagedResponse<TmdbMediaDto>>($"tv/popular?page={page}", cancellationToken);
        return response.Results;
    }
    
    private async Task<T> FetchAsync<T>(string endpoint, CancellationToken cancellationToken)
    {
        var separator = endpoint.Contains('?') ? "&" : "?";
        var url = $"https://api.themoviedb.org/3/{endpoint}{separator}api_key={_apiKey}&language=en-US";
        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(json, JsonOptions)!;
    }
    
    public async Task<TmdbMediaDto> GetMovieDetailsAsync(int tmdbId, CancellationToken cancellationToken = default)
        => await FetchAsync<TmdbMediaDto>($"movie/{tmdbId}", cancellationToken);

    public async Task<TmdbMediaDto> GetTvShowDetailsAsync(int tmdbId, CancellationToken cancellationToken = default)
        => await FetchAsync<TmdbMediaDto>($"tv/{tmdbId}", cancellationToken);

    private class TmdbGenreResponse
    {
        public List<TmdbGenre> Genres { get; set; } = [];
    }

    private class TmdbGenreComparer : IEqualityComparer<TmdbGenre>
    {
        public bool Equals(TmdbGenre? x, TmdbGenre? y) => x?.Id == y?.Id;
        public int GetHashCode(TmdbGenre obj) => obj.Id.GetHashCode();
    }
    public class TmdbPagedResponse<T>
    {
        public List<T> Results { get; set; } = [];
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
}