using System.ComponentModel.DataAnnotations;

namespace Marquee.Application.DTOs.Genre;

public class UpdateGenreDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public int? TmdbId { get; set; }
}