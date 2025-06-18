using System;

namespace Publishing.Core.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Today { get; }
    }
}
