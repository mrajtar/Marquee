using Marquee.Domain.Enums;

namespace Marquee.Domain.Entities;

public abstract class Media
{
    public MediaType Type { get; protected set; }
    
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    public string? OriginalTitle {get; set; }
    public string? Overview { get; set; }
    
    public string? PosterUrl { get; set; }
    public string? BackdropUrl { get; set; }
    
    public Status? Status { get; set; }
    public DateTime? ReleaseDate { get; set; }
    
    public int? TmdbId { get; set; }
    public string? ImdbId { get; set; }

    public ICollection<MediaGenre> MediaGenres { get; set; } = [];
    public ICollection<MediaPerson> MediaPeople { get; set; } = [];
    public ICollection<MediaCountry> MediaCountries { get; set; } = [];
    public ICollection<MediaKeyword> MediaKeywords { get; set; } = [];
    
    public ICollection<Rating> Ratings { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<MediaInteraction> Interactions { get; set; } = [];
}