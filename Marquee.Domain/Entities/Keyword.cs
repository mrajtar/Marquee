namespace Marquee.Domain.Entities;

public class Keyword
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? TmdbId { get; set; }
    
    public ICollection<MediaKeyword> MediaKeywords { get; set; } = [];
}