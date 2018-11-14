using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.Services.ProgramConfiguration;
using BackOnTrack.Services.Proxy;
using BackOnTrack.Services.UserConfiguration;

namespace BackOnTrack.Services
{
    public class ServicesKeyword
    {
        public UserConfigurationSupplier UserConfiguration;
        public ProgramConfigurationSupplier ProgramConfiguration;
        public WebProxy WebProxy;
        public ServicesKeyword()
        {
            ProgramConfiguration = new ProgramConfigurationSupplier();
            UserConfiguration = new UserConfigurationSupplier();
            WebProxy = new WebProxy();
        }
    }
}
