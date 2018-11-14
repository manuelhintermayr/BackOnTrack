using System;
using System.Collections.Generic;
using BackOnTrack.Services.ProgramConfiguration;
using BackOnTrack.Services.UserConfiguration;

namespace BackOnTrack.Services
{
    public class ServicesKeyword
    {
        public UserConfigurationSupplier UserConfiguration;
        public ProgramConfigurationSupplier ProgramConfiguration;
        public WebProxy.WebProxy WebProxy;
        public ServicesKeyword()
        {
            ProgramConfiguration = new ProgramConfigurationSupplier();
            UserConfiguration = new UserConfigurationSupplier();
            WebProxy = new WebProxy.WebProxy();
        }
    }
}
