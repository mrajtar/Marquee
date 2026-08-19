using Marquee.Domain.Enums;

namespace Marquee.Domain.Entities;

public class MediaPerson
{
    public int Id { get; set; }
    public int MediaId  { get; set; }
    public Media Media { get; set; } = null!;
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
    
    public string? CharacterName { get; set; }
    
    public MediaRole Role { get; set; }
    
    public int? CreditOrder { get; set; }
}