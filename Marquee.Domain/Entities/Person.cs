namespace Marquee.Domain.Entities;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Biography { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? ProfileImageUrl { get; set; }
    public int? TmdbId { get; set; }

    public ICollection<MediaPerson> MediaPeople { get; set; } = [];
}