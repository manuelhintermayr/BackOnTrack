using System;

namespace BackOnTrack.WebProxy.Exceptions
{
    public class WebProxyPortAlreadyInUseException : Exception
    {
        public WebProxyPortAlreadyInUseException(string message) : base(message) { }
    }
}
