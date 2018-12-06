using System;

namespace BackOnTrack.Infrastructure.Exceptions
{
    public class SystemLevelException : Exception
    {
        public SystemLevelException(string message) : base(message) { }
    }
}
