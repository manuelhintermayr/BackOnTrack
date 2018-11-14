using System;
using System.Collections.Generic;
using BackOnTrack.Infrastructure.Helpers;
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
            CurrentUserConfiguration configuration = new CurrentUserConfiguration(){ProfileList = new List<Profile>()};

            var jsonConfiguration = JsonConvert.SerializeObject(configuration);
            FileModification.WriteFile(ConfigurationPath, jsonConfiguration);
        }

        public CurrentUserConfiguration OpenConfiguration(string password)
        {
            string configurationContent = FileModification.ReadFile(ConfigurationPath);
            if (configurationContent != "")
            {
                return JsonConvert.DeserializeObject<CurrentUserConfiguration>(configurationContent);
            }

            return null;
        }
    }
}
