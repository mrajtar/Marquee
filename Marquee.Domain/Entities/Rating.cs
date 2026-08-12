namespace Marquee.Domain.Entities;

public class Rating
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int MediaId { get; set; }
    public Media Media { get; set; } = null!;
    
    public decimal Value  { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}