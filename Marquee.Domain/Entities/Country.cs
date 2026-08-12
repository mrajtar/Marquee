namespace Marquee.Domain.Entities;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public int? TmdbId { get; set; }

    public ICollection<MediaCountry> MediaCountries { get; set; } = [];
}