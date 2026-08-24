using System.ComponentModel.DataAnnotations;

namespace Marquee.Application.DTOs.User;

public class UpdateUserDto
{
    [MaxLength(50)]
    public string? DisplayName { get; set; }
    [MaxLength(1000)]
    public string? Bio { get; set; }
    [MaxLength(500)]
    public string? ProfileImageUrl { get; set; }
}