using BackOnTrack.Infrastructure.Helpers;
using BackOnTrack.SharedResources.Models;
using BackOnTrack.WebProxy;
using BackOnTrack.WebProxy.Exceptions;

namespace BackOnTrack.Services.WebProxy
{
    public class RunningWebProxy
    {
        public bool ProxyIsEnabled { get; set; }
        private RunningApplication _runningApplication;
        private LocalWebProxy _webProxy;
        public RunningWebProxy(ServicesKeyword servicesKeyword)
        {
            _runningApplication = RunningApplication.Instance();
            _webProxy = new LocalWebProxy();
            ProxyIsEnabled = servicesKeyword.ProgramConfiguration.Configuration.ProxyEnabled;
            SetupWebProxyForProgramStart(servicesKeyword);
        }

        private void SetupWebProxyForProgramStart(ServicesKeyword servicesKeyword)
        {
            if (ProxyIsEnabled)
            {
                //this should not be the first run of the program
                try
                {
                    _webProxy.LoadProxyProfileFromFileSystem();
                }
                catch (WebProxyNoProfilesFileException e)
                {
                    Messages.CreateMessageBox(
                        e.Message,
                        "Back on Track - proxy file does not exit", true);
                    Messages.CreateMessageBox(
                        "Proxy is shutting down.",
                        "Back on Track - Proxy shutting down", true);

                    ProxyIsEnabled = false;
                    servicesKeyword.ProgramConfiguration.TempConfiguration.ProxyEnabled = false;
                    servicesKeyword.ProgramConfiguration.SaveCurrentConfiguration();
                }
                
                
            }
            else
            {
                //could be the first run of the program
            }

        }

        public void Start()
        {
            //webProxy.StartProxy();
        }

        public void Quit()
        {
            //webProxy.QuitProxy();
        }

        public void UpdateConfiguration(CurrentUserConfiguration newConfiguration)
        {
            //...
        }

        public void Dispose()
        {
            _webProxy.Dispose();
        }
    }
}
