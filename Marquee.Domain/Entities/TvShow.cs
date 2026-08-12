using Marquee.Domain.Enums;

namespace Marquee.Domain.Entities;

public class TvShow : Media
{
    public TvShow()
    {
        Type = MediaType.TvShow;
    }
    public DateTime? LastAirDate  { get; set; }
    public int? NumberOfSeasons { get; set; }
    public int? NumberOfEpisodes { get; set; }
}