namespace Publishing.Services;

using System;
using Publishing.Core.DTOs;

public interface IProfileEventsPublisher
{
    event Action<ProfileDto>? ProfileUpdated;

    void PublishProfileUpdated(ProfileDto profile);
}
