using System;
using BackOnTrack.Infrastructure.Helpers;
using BackOnTrack.Services.SystemLevelConfiguration;
using BackOnTrack.SharedResources.Infrastructure.Helpers;
using Newtonsoft.Json;

namespace BackOnTrack.Services.ProgramConfiguration
{
    public class ProgramConfigurationSupplier
    {
        public RunningApplication _runningApplication;
        public string ConfigurationPath;
        public CurrentProgramConfiguration Configuration;
        public CurrentProgramConfiguration TempConfiguration;
        public AutorunHelper Autorun;

        public ProgramConfigurationSupplier()
        {
            _runningApplication = RunningApplication.Instance();
            ConfigurationPath =
                $"{RunningApplication.ProgramSettingsPath()}\\.backOnTrack\\config.settings";
            Autorun = new AutorunHelper();

            SetCurrentConfigurationFromConfig();
        }

        public void SetCurrentConfigurationFromConfig()
        {
            if (FileModification.FileExists(ConfigurationPath))
            {
                string configurationContent = FileModification.ReadFile(ConfigurationPath);
                if (configurationContent == "")
                {
                    CreateNewConfiguration();
                }
                else
                {
                    Configuration = JsonConvert.DeserializeObject<CurrentProgramConfiguration>(configurationContent);
                }
            }
            else
            {
                FileModification.CreateFolderIfNotExists(ConfigurationPath.Replace("\\config.settings", ""));
                CreateNewConfiguration();
            }

            CopyCurrentConfigurationToTempConfig();
        }

        public void SetCurrentConfigurationToDefault()
        {
            if (!FileModification.FileExists(ConfigurationPath))
            {
                FileModification.CreateFolderIfNotExists(ConfigurationPath.Replace("\\config.settings", ""));
            }
            CreateNewConfiguration();

            CopyCurrentConfigurationToTempConfig();
        }

        public void SaveCurrentConfiguration()
        {
            CopyTempConfigurationToCurrentConfig();

            if (!FileModification.FileExists(ConfigurationPath))
            {
                FileModification.CreateFolderIfNotExists(ConfigurationPath.Replace("\\config.settings", ""));
            }
            SaveConfiguration(Configuration);

            CopyCurrentConfigurationToTempConfig();
        }

        public void RevertChangesFromCurrentConfig()
        {
            CopyCurrentConfigurationToTempConfig();
        }

        private void CreateNewConfiguration()
        {
            Configuration = new CurrentProgramConfiguration()
            {
                ProxyEnabled = false,
                ProxyPortNumber = "8000",
                AutoRunEnabled = true
            };
            SaveConfiguration(Configuration);
        }

        private void SaveConfiguration(CurrentProgramConfiguration config)
        {
            var jsonConfiguration = JsonConvert.SerializeObject(config);
            FileModification.WriteFile(ConfigurationPath, jsonConfiguration);
        }

        private void CopyCurrentConfigurationToTempConfig()
        {
            if (TempConfiguration == null)
            {
                TempConfiguration = new CurrentProgramConfiguration();
            }

            TempConfiguration.ProxyEnabled = Configuration.ProxyEnabled;
            TempConfiguration.ProxyPortNumber = Configuration.ProxyPortNumber;
            TempConfiguration.AutoRunEnabled = Configuration.AutoRunEnabled;

            LoadingConfiguration();
        }

        private void CopyTempConfigurationToCurrentConfig()
        {
            Configuration.ProxyEnabled = TempConfiguration.ProxyEnabled;
            Configuration.ProxyPortNumber = TempConfiguration.ProxyPortNumber;
            Configuration.AutoRunEnabled = TempConfiguration.AutoRunEnabled;
        }


        private void LoadingConfiguration()
        {
            if (Configuration.AutoRunEnabled)
            {
                if (!Autorun.AutorunIsEnabled())
                {
                    Autorun.AddToAutorun();
                }
            }
            else
            {
                if (Autorun.AutorunIsEnabled())
                {
                    Autorun.RemoveFromAutorun();
                }
            }

            if (_runningApplication.Services != null)
            {
                //only if services are already initialized
                if (Configuration.ProxyEnabled)
                {
                    _runningApplication.Services.WebProxy.CreateEmptyProfileConfigurationIfNotExists();
                    int newProxyPortNumber = Int32.Parse(Configuration.ProxyPortNumber);

                    if (_runningApplication.Services.WebProxy.ProxyIsRunning)
                    {
                        //proxy already running
                        if (_runningApplication.Services.WebProxy.GetPortNumber() != newProxyPortNumber)
                        {
                            _runningApplication.Services.WebProxy.Quit();
                            _runningApplication.Services.WebProxy.UpdatePortNumber(newProxyPortNumber);
                            _runningApplication.Services.WebProxy.Start();
                        }
                    }
                    else
                    {
                        //must start proxy
                        _runningApplication.Services.WebProxy.UpdatePortNumber(newProxyPortNumber);
                        _runningApplication.Services.WebProxy.Start();
                    }
                }
                else
                {
                    if (_runningApplication.Services.WebProxy.ProxyIsRunning)
                    {
                        _runningApplication.Services.WebProxy.Quit();
                    }
                }
            }
        }
    }
}
