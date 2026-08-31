namespace Marquee.Application.DTOs.MediaList;

public class MediaListDetailsDto : MediaListDto
{
    public IReadOnlyList<MediaListItemDto> Items { get; set; } = [];
}

public class MediaListItemDto
{
    public int MediaId { get; set; }
    public string Title { get; set; } = null!;
    public string? PosterUrl { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public DateTime AddedAt { get; set; }
}