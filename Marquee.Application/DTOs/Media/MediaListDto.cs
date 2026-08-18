using Marquee.Domain.Enums;

namespace Marquee.Application.DTOs.Media;

public class MediaListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? PosterUrl { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public MediaType MediaType { get; set; }
}