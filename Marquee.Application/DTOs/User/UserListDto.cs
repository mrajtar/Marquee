namespace Marquee.Application.DTOs.User;

public class UserListDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string? DisplayName { get; set; }
    public string? ProfileImageUrl { get; set; }
}