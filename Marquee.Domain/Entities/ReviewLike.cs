namespace Marquee.Domain.Entities;

public class ReviewLike
{
    public int UserId  { get; set; }
    public User User { get; set; } = null!;
    public int ReviewId { get; set; }
    public Review Review { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}