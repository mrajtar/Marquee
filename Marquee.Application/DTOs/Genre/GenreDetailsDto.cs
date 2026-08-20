namespace Marquee.Application.DTOs.Genre;

public class GenreDetailsDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? TmdbId { get; set; }
}