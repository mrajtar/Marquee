using AutoMapper;
using Marquee.Application.DTOs.Media;
using Marquee.Domain.Entities;

namespace Marquee.Application.Mapping;

public class MediaProfile : Profile
{
    public MediaProfile()
    {
        CreateMap<Media, MediaListDto>();
        CreateMap<Genre, GenreDto>();
        CreateMap<Keyword, KeywordDto>();
        CreateMap<Media, MediaDetailsDto>()
            .ForMember(
                dest => dest.Genres,
                opt => opt.MapFrom(
                    src => src.MediaGenres.Select(x => x.Genre)));

        CreateMap<CreateMediaDto, Movie>()
            .ForMember(
                dest => dest.MediaGenres,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.MediaKeywords,
                opt => opt.Ignore());
        CreateMap<CreateMediaDto, TvShow>()
            .ForMember(
                dest => dest.MediaGenres,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.MediaKeywords,
                opt => opt.Ignore());
        CreateMap<UpdateMediaDto, Movie>()
            .ForMember(
                dest => dest.MediaGenres,
                opt => opt.Ignore())
            .ForMember(dest => dest.MediaKeywords,
                opt => opt.Ignore());
        CreateMap<UpdateMediaDto, TvShow>()
            .ForMember(
                dest => dest.MediaGenres,
                opt => opt.Ignore())
            .ForMember(dest => dest.MediaKeywords,
                opt => opt.Ignore());
    }
}