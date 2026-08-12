namespace Marquee.Domain.Entities;

public class MediaKeyword
{
    public int MediaId { get; set; }
    public Media Media { get; set; } = null!;
    public int KeywordId { get; set; }
    public Keyword Keyword { get; set; } = null!;
}