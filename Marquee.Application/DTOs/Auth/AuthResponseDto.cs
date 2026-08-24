using Marquee.Application.DTOs.User;

namespace Marquee.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public UserDetailsDto User { get; set; } = null!;
}