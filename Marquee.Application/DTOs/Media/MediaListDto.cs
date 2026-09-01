using Marquee.Domain.Enums;

namespace Marquee.Application.DTOs.Media;

public class MediaListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? PosterUrl { get; set; }
    public string? BackdropUrl { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public double? AverageRating { get; set; }
    public int RatingCount { get; set; }
    public MediaType MediaType { get; set; }
}