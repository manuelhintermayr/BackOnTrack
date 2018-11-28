using System;
using BackOnTrack.Infrastructure.Helpers;
using Newtonsoft.Json;

namespace BackOnTrack.Services.ProgramConfiguration
{
    public class ProgramConfigurationSupplier
    {
        public string ConfigurationPath;
        public CurrentProgramConfiguration Configuration;
        public CurrentProgramConfiguration TempConfiguration;
        public AutorunHelper Autorun;

        public ProgramConfigurationSupplier()
        {
            ConfigurationPath =
                $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\.backOnTrack\\config.settings";
            //todo: ^ make more configurable for testing 
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

        public void SaveCurrentConfiguration()
        {
            CopyTempConfigurationToCurrentConfig();

            if (!FileModification.FileExists(ConfigurationPath))
            {
                FileModification.CreateFolderIfNotExists(ConfigurationPath.Replace("\\config.settings", ""));
            }

            SaveConfiguration(Configuration);
        }

        public void RevertChangesFromCurrentConfig()
        {
            CopyCurrentConfigurationToTempConfig();
        }

        private void CreateNewConfiguration()
        {
            Configuration = new CurrentProgramConfiguration() { ProxyEnabled = false, AutoRunEnabled = true};
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
            TempConfiguration.AutoRunEnabled = Configuration.AutoRunEnabled;

            LoadingConfiguration();
        }

        private void CopyTempConfigurationToCurrentConfig()
        {
            Configuration.ProxyEnabled = TempConfiguration.ProxyEnabled;
            Configuration.AutoRunEnabled = TempConfiguration.AutoRunEnabled;
        }


        private void LoadingConfiguration()
        {
            if (Configuration.AutoRunEnabled)
            {
                if (!Autorun.AutorunIsEnabled())
                {
                    bool worked = Autorun.AddToAutorun();
                }
            }
            else
            {
                if (Autorun.AutorunIsEnabled())
                {
                    bool worked = Autorun.RemoveFromAutorun();
                }
            }

            if (Configuration.ProxyEnabled)
            {
                //turn on
            }
            else
            {
                //turn off
            }

        }
    }
}
