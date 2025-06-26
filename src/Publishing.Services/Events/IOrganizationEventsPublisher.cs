namespace Publishing.Services;

using System;
using Publishing.Core.DTOs;

public interface IOrganizationEventsPublisher
{
    event Action<OrganizationDto>? OrganizationUpdated;

    void PublishOrganizationUpdated(OrganizationDto organization);
}
