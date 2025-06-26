namespace Publishing.Services;

using System;
using Publishing.Core.DTOs;

public class OrganizationEventsPublisher : IOrganizationEventsPublisher
{
    public event Action<OrganizationDto>? OrganizationUpdated;

    public void PublishOrganizationUpdated(OrganizationDto organization)
    {
        OrganizationUpdated?.Invoke(organization);
    }
}
