using AutoMapper;
using Publishing.Core.DTOs;
using Publishing.Core.Commands;

namespace Publishing.AppLayer.Mapping;

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateMap<UpdateOrganizationDto, UpdateOrganizationCommand>();
        CreateMap<UpdateOrganizationDto, CreateOrganizationCommand>();
    }
}
