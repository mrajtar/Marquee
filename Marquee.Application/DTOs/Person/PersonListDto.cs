namespace Marquee.Application.DTOs.Person;

public class PersonListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ProfileImageUrl { get; set; }
}