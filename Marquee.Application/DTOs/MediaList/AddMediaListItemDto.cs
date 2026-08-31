using System.ComponentModel.DataAnnotations;

namespace Marquee.Application.DTOs.MediaList;

public class AddMediaListItemDto
{
    [Required]
    public int MediaId { get; set; }
}