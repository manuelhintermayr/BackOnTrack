using System;
using System.Collections.Generic;
using BackOnTrack.Services.ProgramConfiguration;
using BackOnTrack.Services.SystemLevelConfiguration;
using BackOnTrack.Services.UserConfiguration;

namespace BackOnTrack.Services
{
    public class ServicesKeyword
    {
        public UserConfigurationSupplier UserConfiguration;
        public ProgramConfigurationSupplier ProgramConfiguration;
        public WebProxy.RunningWebProxy WebProxy;
        public SystemLevelConfigurationSupplier SystemLevelConfiguration;
        public ServicesKeyword()
        {
            ProgramConfiguration = new ProgramConfigurationSupplier();
            UserConfiguration = new UserConfigurationSupplier();
            SystemLevelConfiguration = new SystemLevelConfigurationSupplier();
            WebProxy = new WebProxy.RunningWebProxy();
        }
    }
}
