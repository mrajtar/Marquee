using Marquee.Domain.Enums;

namespace Marquee.Domain.Entities;

public class MediaInteraction
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int MediaId  { get; set; }
    public Media Media { get; set; } = null!;
    
    public InteractionType InteractionType { get; set; }
    
    public DateTime CreatedAt  { get; set; }
}