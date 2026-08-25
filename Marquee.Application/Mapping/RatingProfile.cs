using AutoMapper;
using Marquee.Application.DTOs.Rating;
using Marquee.Domain.Entities;

namespace Marquee.Application.Mapping;

public class RatingProfile : Profile
{
    public RatingProfile()
    {
        CreateMap<Rating, RatingDto>();
    }
}