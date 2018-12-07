using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOnTrack.WebProxy.Exceptions
{
    public class WebProxyBrokenProfileConfigurationException : Exception
    {
        public WebProxyBrokenProfileConfigurationException(string message) : base(message) { }
    }
}
