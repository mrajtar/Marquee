using System.ComponentModel.DataAnnotations.Schema;

namespace Marquee.Domain.Entities;

public class MediaList
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public bool IsPublic  { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    [NotMapped]
    public bool ContainsMedia { get; set; }

    public ICollection<MediaListItem> Items { get; set; } = [];
}