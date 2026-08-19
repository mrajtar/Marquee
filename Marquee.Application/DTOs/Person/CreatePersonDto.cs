using System.ComponentModel.DataAnnotations;

namespace Marquee.Application.DTOs.Person;

public class CreatePersonDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;
    [StringLength(500)]
    public string? ProfileUrl { get; set; }
    [MaxLength(10000)]
    public string? Biography { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    [MaxLength(500)]
    public string? PlaceOfBirth { get; set; }
    public int? TmdbId { get; set; }
}