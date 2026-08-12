namespace Marquee.Domain.Entities;

public class UserTopMedia
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int MediaId { get; set; }
    public Media Media { get; set; } = null!;
    
    public int Position {get; set;}
    public string? Note { get; set; }
}