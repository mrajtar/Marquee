namespace Marquee.Application.DTOs.Keyword;

public class KeywordDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? TmdbId { get; set; }
}