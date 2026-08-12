namespace Marquee.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;
    public string? DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? ProfileImageUrl { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public ICollection<Rating> Ratings { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<MediaInteraction> MediaInteractions { get; set; } = [];
    public ICollection<MediaList> MediaLists { get; set; } = [];
    public ICollection<UserTopMedia> TopMedia { get; set; } = [];
}