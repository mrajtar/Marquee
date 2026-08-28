namespace Marquee.Domain.Entities;

public class Review
{
    public int Id { get; set; }
    
    public int UserId  { get; set; }
    public User User { get; set; } = null!;
    public int MediaId { get; set; }
    public Media Media { get; set; } = null!;

    public string Content { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public bool ContainsSpoilers { get; set; }
    public ICollection<ReviewLike> Likes { get; set; } = [];
}