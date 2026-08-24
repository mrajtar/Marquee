using AutoMapper;
using Marquee.Application.DTOs.User;
using Marquee.Domain.Entities;

namespace Marquee.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserListDto>();
        CreateMap<User, UserDetailsDto>();
        CreateMap<UserDetailsDto, User>();
    }
}