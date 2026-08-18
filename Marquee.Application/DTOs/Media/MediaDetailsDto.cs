using Marquee.Domain.Entities;
using Marquee.Domain.Enums;

namespace Marquee.Application.DTOs.Media;

public class MediaDetailsDto
{
    public int Id  { get; set; }
    public string Title { get; set; } = null!;
    public string? OriginalTitle { get; set; }
    public string? Overview { get; set; }
    public string? PosterUrl { get; set; }
    public string? BackdropUrl  { get; set; }
    public Status? Status { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public int? TmdbId { get; set; }
    public string? ImdbId { get; set; }
    public MediaType MediaType { get; set; }
    public List<Genre> Genres { get; set; } = [];
}