using System.ComponentModel.DataAnnotations;

namespace Marquee.Application.DTOs.Review;

public class CreateReviewDto
{
    [Required]
    [MaxLength(10000)]
    public string Content { get; set; } = null!;
}