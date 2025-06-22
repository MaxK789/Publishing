using AutoMapper;
using Publishing.Core.DTOs;
using Publishing.Core.Commands;

namespace Publishing.AppLayer.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserDto, RegisterUserCommand>();
    }
}
