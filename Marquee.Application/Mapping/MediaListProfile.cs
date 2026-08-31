using AutoMapper;
using Marquee.Application.DTOs.MediaList;
using Marquee.Domain.Entities;

namespace Marquee.Application.Mapping;

public class MediaListProfile : Profile
{
    public MediaListProfile()
    {
        CreateMap<MediaList, MediaListDto>()
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.Items.Count));

        CreateMap<MediaList, MediaListDetailsDto>()
            .IncludeBase<MediaList, MediaListDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<MediaListItem, MediaListItemDto>()
            .ForMember(dest => dest.MediaId, opt => opt.MapFrom(src => src.MediaId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Media.Title))
            .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.Media.PosterUrl))
            .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Media.ReleaseDate))
            .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => src.AddedAt));

        CreateMap<CreateMediaListDto, MediaList>();
        CreateMap<UpdateMediaListDto, MediaList>();
    }
}