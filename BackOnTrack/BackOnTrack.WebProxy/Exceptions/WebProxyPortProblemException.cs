using System;

namespace BackOnTrack.WebProxy.Exceptions
{
    public class WebProxyPortProblemException : Exception
    {
        public WebProxyPortProblemException(string message) : base(message) { }
    }
}
