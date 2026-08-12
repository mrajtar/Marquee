using Marquee.Domain.Enums;

namespace Marquee.Domain.Entities;

public class Movie : Media
{
    public Movie()
    {
        Type = MediaType.Movie;
    }
    public int? RuntimeMinutes { get; set; }
    public decimal? Budget { get; set; }
    public decimal? Revenue  { get; set; }
}