namespace Marquee.Application.DTOs.Person;

public class PersonDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ProfileUrl  { get; set; }
    public string? Biography  { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public string? PlaceOfBirth { get; set; }
    public int? TmdbId { get; set; }
}