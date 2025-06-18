using System;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure
{
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime Today => DateTime.Today;
    }
}
