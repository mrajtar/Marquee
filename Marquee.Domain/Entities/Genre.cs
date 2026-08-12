namespace Marquee.Domain.Entities;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? TmdbId { get; set; }

    public ICollection<MediaGenre> MediaGenres { get; set; } = [];
}