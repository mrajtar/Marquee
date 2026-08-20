using AutoMapper;
using Marquee.Application.DTOs.Genre;
using Marquee.Domain.Entities;

namespace Marquee.Application.Mapping;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<Genre, GenreListDto>();
        CreateMap<Genre, GenreDetailsDto>();
        CreateMap<CreateGenreDto, Genre>();
        CreateMap<UpdateGenreDto, Genre>();
    }
}