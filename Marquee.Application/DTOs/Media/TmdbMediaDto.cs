namespace Marquee.Application.DTOs.Media;

public class TmdbMediaDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Name { get; set; }
    public string? OriginalTitle { get; set; }
    public string? OriginalName { get; set; }
    public string? Overview { get; set; }
    public string? PosterPath { get; set; }
    public string? BackdropPath { get; set; }
    public string? ReleaseDate { get; set; }
    public string? FirstAirDate { get; set; }
    public int? Runtime { get; set; }
    public decimal? Budget { get; set; }    
    public decimal? Revenue { get; set; }
    public List<int> GenreIds { get; set; } = [];
    public List<TmdbGenreRef> Genres { get; set; } = [];
    public List<string> OriginCountry { get; set; } = [];
    public string? Status { get; set; }
    public int? NumberOfSeasons { get; set; }
    public int? NumberOfEpisodes { get; set; }
    public string? ImdbId { get; set; }
}
public class TmdbGenreRef
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}