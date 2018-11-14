using System;
using BackOnTrack.Infrastructure.Helpers;
using Newtonsoft.Json;

namespace BackOnTrack.Services.ProgramConfiguration
{
    public class ProgramConfigurationSupplier
    {
        public string ConfigurationPath;
        public CurrentProgramConfiguration Settings;

        public ProgramConfigurationSupplier()
        {
            ConfigurationPath =
                $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\.backOnTrack\\config.settings";
            //todo: ^ make more configurable for testing 

           SetCurrentConfiguration();
        }

        private void SetCurrentConfiguration()
        {
            if (FileModification.FileExists(ConfigurationPath))
            {
                string configuration = FileModification.ReadFile(ConfigurationPath);
                if (configuration == "")
                {
                    CreateNewConfiguration();
                }
                else
                {
                    Settings = JsonConvert.DeserializeObject<CurrentProgramConfiguration>(configuration);
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
            Settings = new CurrentProgramConfiguration() { ProxyEnabled = false };
            var jsonSettings = JsonConvert.SerializeObject(Settings);
            FileModification.WriteFile(ConfigurationPath, jsonSettings);
        }
    }
}
