using System.ComponentModel.DataAnnotations;
using Marquee.Domain.Enums;

namespace Marquee.Application.DTOs.Media;

public class UpdateMediaDto
{
    [Required]
    [MaxLength(200)]
    public string? Title { get; set; } = null!;
    [MaxLength(200)]
    public string? OriginalTitle { get; set; }
    [MaxLength(5000)]
    public string? Overview { get; set; }
    public string? PosterUrl { get; set; }
    public string? BackdropUrl { get; set; }
    public Status? Status { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public int? TmdbId { get; set; }
    public string? ImdbId { get; set; }
    public List<int> GenreIds { get; set; } = [];
    public List<int> KeywordIds { get; set; } = [];
}