using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOnTrack.WebProxy.Exceptions
{
    public class WebProxyPortProblemException : Exception
    {
        public WebProxyPortProblemException(string message) : base(message) { }
    }
}
