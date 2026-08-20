using System.ComponentModel.DataAnnotations;

namespace Marquee.Application.DTOs.Keyword;

public class UpdateKeywordDto
{
    [Required]
    [MaxLength(250)]
    public string Name { get; set; } = null!;
    public int? TmdbId { get; set; }
}