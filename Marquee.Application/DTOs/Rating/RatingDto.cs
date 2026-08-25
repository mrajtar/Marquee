namespace Marquee.Application.DTOs.Rating;

public class RatingDto
{
    public int MediaId  { get; set; }
    public int Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}