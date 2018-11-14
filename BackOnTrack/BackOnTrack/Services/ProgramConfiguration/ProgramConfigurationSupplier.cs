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

        public ProgramConfigurationSupplier()
        {
            ConfigurationPath =
                $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\.backOnTrack\\config.settings";
            //todo: ^ make more configurable for testing 

            SetCurrentConfiguration();
            CopyCurrentConfiguration();
        }

        private void SetCurrentConfiguration()
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
        }

        private void CreateNewConfiguration()
        {
            Configuration = new CurrentProgramConfiguration() { ProxyEnabled = false };
            var jsonConfiguration = JsonConvert.SerializeObject(Configuration);
            FileModification.WriteFile(ConfigurationPath, jsonConfiguration);
        }

        public void CopyCurrentConfiguration()
        {
            TempConfiguration = (CurrentProgramConfiguration)Configuration.Clone();
        }
    }
}
