namespace Marquee.Domain.Entities;

public class MediaCountry
{
    public int MediaId  { get; set; }
    public Media Media { get; set; } = null!;
    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;
}