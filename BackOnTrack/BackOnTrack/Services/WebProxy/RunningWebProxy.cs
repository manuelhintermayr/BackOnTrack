using System;
using BackOnTrack.Infrastructure.Helpers;
using BackOnTrack.SharedResources.Models;
using BackOnTrack.WebProxy;
using BackOnTrack.WebProxy.Exceptions;

namespace BackOnTrack.Services.WebProxy
{
    public class RunningWebProxy
    {
        public bool ProxyIsEnabled
        {
            get
            {
                return _webProxy.ProxyIsEnabled;
            }
            set { _webProxy.ProxyIsEnabled = value; }
        }
        public bool ProxyIsRunning { get; set; }
        private RunningApplication _runningApplication;
        private LocalWebProxy _webProxy;
        public RunningWebProxy(ServicesKeyword servicesKeyword)
        {
            _runningApplication = RunningApplication.Instance();
            _webProxy = new LocalWebProxy();
            ProxyIsEnabled = servicesKeyword.ProgramConfiguration.Configuration.ProxyEnabled;
            ProxyIsRunning = false;
            SetupWebProxyForProgramStart(servicesKeyword);
        }

        #region Proxy configuration for program start
        private void SetupWebProxyForProgramStart(ServicesKeyword servicesKeyword)
        {
            if (ProxyIsEnabled)
            {
                try
                {
                    _webProxy.LoadProxyProfileFromFileSystem();
                    UpdatePortNumber(Int32.Parse(servicesKeyword.ProgramConfiguration.Configuration.ProxyPortNumber));
                    Start();
                }
                catch (WebProxyNoProfilesFileException e)
                {
                    string errorTitle = "Back on Track - proxy file does not exit";
                    string errorMessage = e.Message;

                    ProgramStartShuttingDownProxy(errorTitle, errorMessage, servicesKeyword);
                }
                catch (WebProxyBrokenProfileConfigurationException e)
                {
                    string errorTitle = "Back on Track - Proxy profile configuration file is broken";
                    string errorMessage = e.Message;

                    ProgramStartShuttingDownProxy(errorTitle, errorMessage, servicesKeyword);
                }
                catch (WebProxyPortAlreadyInUseException e)
                {
                    string errorTitle = "Back on Track - Proxy port number already in use";
                    string errorMessage = e.Message;

                    ProgramStartShuttingDownProxy(errorTitle, errorMessage, servicesKeyword);
                }
            }
            else
            {
                try
                {
                    //still load settings
                    _webProxy.LoadProxyProfileFromFileSystem();
                }
                catch (Exception) { }
            }
        }
        private void ProgramStartShuttingDownProxy(string errorTitle, string errorMessage, ServicesKeyword servicesKeyword)
        {
            Messages.CreateMessageBox(
                errorMessage,
                errorTitle, true);

            Messages.CreateMessageBox(
                "Proxy is shutting down.",
                "Back on Track - Proxy shutting down", true);

            ProxyIsEnabled = false;
            servicesKeyword.ProgramConfiguration.TempConfiguration.ProxyEnabled = false;
            servicesKeyword.ProgramConfiguration.SaveCurrentConfiguration();
        }
        #endregion

        #region Proxy configuration
        public void UpdateConfiguration(CurrentUserConfiguration newConfiguration)
        {
            if (ProxyIsRunning) // mutex for proxyConfiguration
            {
                ProxyIsEnabled = false;
            }
            
            _webProxy.ApplyUserConfigurationOnProxy(newConfiguration);

            if (ProxyIsRunning)
            {
                ProxyIsEnabled = true;
            }
        }
        public void UpdatePortNumber(int portNumber)
        {
            _webProxy.SetPortNumber(portNumber);
        }

        public int GetPortNumber()
        {
            return _webProxy.GetPortNumber();
        }
        #endregion

        #region Start Quit and Dispose Proxy
        public void Start()
        {
            _webProxy.StartProxy();
            ProxyIsRunning = true;
        }
        public void Quit()
        {
            _webProxy.QuitProxy();
            ProxyIsRunning = false;
        }
        public void Dispose()
        {
            _webProxy.Dispose();
        }
        #endregion

        public void CreateEmptyProfileConfigurationIfNotExists()
        {
            _webProxy.CreateEmptyProfileConfigurationIfNotExists();
        }
    }
}
