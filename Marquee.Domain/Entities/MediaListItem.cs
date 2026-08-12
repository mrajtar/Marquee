namespace Marquee.Domain.Entities;

public class MediaListItem
{
    public int MediaListId  { get; set; }
    public MediaList MediaList { get; set; } = null!;
    
    public int MediaId  { get; set; }
    public Media Media { get; set; } = null!;
    
    public DateTime AddedAt { get; set; }
}