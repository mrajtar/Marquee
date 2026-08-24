using System.ComponentModel.DataAnnotations;

namespace Marquee.Application.DTOs.Auth;

public class RegisterDto
{
    [Required]
    [MaxLength(32)]
    public string Username { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    [MinLength(8)]
    public string Password { get; set; } = null!;
    [MaxLength(50)]
    public string? DisplayName { get; set; }
}