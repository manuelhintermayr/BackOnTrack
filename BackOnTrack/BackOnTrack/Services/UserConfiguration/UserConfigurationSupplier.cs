using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.Infrastructure.Helpers;
using BackOnTrack.Properties;
using BackOnTrack.Services.ProgramConfiguration;
using Newtonsoft.Json;

namespace BackOnTrack.Services.UserConfiguration
{
    public class UserConfigurationSupplier
    {
        public string ConfigurationPath;

        public UserConfigurationSupplier()
        {
            ConfigurationPath = 
                $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\.backOnTrack\\profiles.settings";
            //todo: ^make configurable for testing
        }

        public bool CheckPassword(string password)
        {
            return password == "admin";
            //todo: ^update
        }

        public bool ConfigurationIsAlreadyCreated()
        {
            return FileModification.FileExists(ConfigurationPath);
        }

        public void CreateNewConfiguration(string password)
        {
            CurrentUserConfiguration configuration = new CurrentUserConfiguration(){profileList = new List<Profile>()};

            var jsonConfiguration = JsonConvert.SerializeObject(configuration);
            FileModification.WriteFile(ConfigurationPath, jsonConfiguration);
        }

        public CurrentUserConfiguration OpenConfiguration(string password)
        {
            string configuration = FileModification.ReadFile(ConfigurationPath);
            if (configuration == "")
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<CurrentUserConfiguration>(configuration);
            }
        }
    }
}
