using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.Infrastructure.Helpers;

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
            //todo: ^ add more configurable for testing 

            if (FileModification.FileExists(ConfigurationPath))
            {
                string configuration = FileModification.ReadFile(ConfigurationPath);
            }
            else
            {
                FileModification.CreateFolderIfNotExists(ConfigurationPath.Replace("\\config.settings",""));
                Settings = new CurrentProgramConfiguration() {ProxyEnabled = false};

                var result = File.Create(ConfigurationPath);
            }
        }
    }
}
