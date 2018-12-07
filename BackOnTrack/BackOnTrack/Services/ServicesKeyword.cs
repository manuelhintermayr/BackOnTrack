using BackOnTrack.Services.ProgramConfiguration;
using BackOnTrack.Services.SystemLevelConfiguration;
using BackOnTrack.Services.UserConfiguration;
using BackOnTrack.Services.WebProxy;

namespace BackOnTrack.Services
{
    public class ServicesKeyword
    {
        public UserConfigurationSupplier UserConfiguration;
        public ProgramConfigurationSupplier ProgramConfiguration;
        public RunningWebProxy WebProxy;
        public SystemLevelConfigurationSupplier SystemLevelConfiguration;
        public ServicesKeyword()
        {
            ProgramConfiguration = new ProgramConfigurationSupplier();
            UserConfiguration = new UserConfigurationSupplier();
            SystemLevelConfiguration = new SystemLevelConfigurationSupplier();
            WebProxy = new RunningWebProxy(this);
        }
    }
}
