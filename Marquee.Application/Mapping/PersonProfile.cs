using AutoMapper;
using Marquee.Application.DTOs.Person;
using Marquee.Domain.Entities;

namespace Marquee.Application.Mapping;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<Person, PersonListDto>();
        CreateMap<Person, PersonDetailsDto>();
        CreateMap<CreatePersonDto, Person>();
        CreateMap<UpdatePersonDto, Person>();
    }
}