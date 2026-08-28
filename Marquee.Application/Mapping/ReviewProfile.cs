using System.Net.NetworkInformation;
using AutoMapper;
using Marquee.Application.DTOs.Review;
using Marquee.Domain.Entities;

namespace Marquee.Application.Mapping;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewListDto>()
            .ForMember(dest => dest.Username,
                opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.DisplayName,
                opt => opt.MapFrom(src => src.User.DisplayName))
            .ForMember(dest => dest.ProfileImageUrl,
                opt => opt.MapFrom(src => src.User.ProfileImageUrl));
    }
}