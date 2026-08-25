using System.ComponentModel.DataAnnotations;

namespace Marquee.Application.DTOs.Rating;

public class SetRatingDto
{
    [Range(1, 20)]
    public int Value { get; set; }
}