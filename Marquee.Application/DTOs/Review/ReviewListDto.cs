namespace Marquee.Application.DTOs.Review;

public class ReviewListDto
{
    public int Id { get; set; }
    public int MediaId { get; set; }
    public string Username { get; set; } = null!;
    public string? DisplayName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}