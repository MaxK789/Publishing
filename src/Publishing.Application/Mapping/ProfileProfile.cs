using AutoMapper;
using Publishing.Core.DTOs;
using Publishing.Core.Commands;

namespace Publishing.AppLayer.Mapping;

public class ProfileProfile : Profile
{
    public ProfileProfile()
    {
        CreateMap<UpdateProfileDto, UpdateProfileCommand>();
    }
}
