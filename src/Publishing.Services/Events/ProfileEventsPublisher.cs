namespace Publishing.Services;

using System;
using Publishing.Core.DTOs;

public class ProfileEventsPublisher : IProfileEventsPublisher
{
    public event Action<ProfileDto>? ProfileUpdated;

    public void PublishProfileUpdated(ProfileDto profile)
    {
        ProfileUpdated?.Invoke(profile);
    }
}
