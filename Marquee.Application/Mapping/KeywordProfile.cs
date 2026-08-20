using AutoMapper;
using Marquee.Application.DTOs.Keyword;
using Marquee.Domain.Entities;

namespace Marquee.Application.Mapping;

public class KeywordProfile : Profile
{
    public KeywordProfile()
    {
        CreateMap<Keyword, KeywordListDto>();
        CreateMap<Keyword, KeywordDetailsDto>();
        CreateMap<CreateKeywordDto, Keyword>();
        CreateMap<UpdateKeywordDto, Keyword>();
    }
}