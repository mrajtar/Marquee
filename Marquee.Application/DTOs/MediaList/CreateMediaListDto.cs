using System.ComponentModel.DataAnnotations;

namespace Marquee.Application.DTOs.MediaList;

public class CreateMediaListDto
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(600)]
    public string? Description { get; set; }

    public bool IsPublic { get; set; } = true;
}