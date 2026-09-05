namespace Marquee.Application.DTOs.User;

public class UserDetailsDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string? DisplayName { get; set; }
    public string? Bio  { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsAdmin { get; set; }
}