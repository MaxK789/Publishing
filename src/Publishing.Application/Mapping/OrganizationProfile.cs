using AutoMapper;
using Publishing.Core.DTOs;
using Publishing.Core.Commands;

namespace Publishing.AppLayer.Mapping;

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateMap<UpdateOrganizationDto, UpdateOrganizationCommand>();
        CreateMap<UpdateOrganizationDto, CreateOrganizationCommand>()
            .ForMember(d => d.PersonId, o => o.MapFrom(s => s.Id));
        CreateMap<CreateOrganizationDto, CreateOrganizationCommand>();
    }
}
