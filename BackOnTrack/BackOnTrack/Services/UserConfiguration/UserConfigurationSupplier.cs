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
            try
            {
                OpenConfiguration(password);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool ConfigurationIsAlreadyCreated()
        {
            return FileModification.FileExists(ConfigurationPath);
        }

        public void CreateNewConfiguration(string password)
        {
            CurrentUserConfiguration configuration = new CurrentUserConfiguration(){ProfileList = new List<Profile>()};

            var jsonConfiguration = JsonConvert.SerializeObject(configuration);
            string encryptedConfiguration = EncryptingHelper.Encrypt(jsonConfiguration, password);

            FileModification.WriteFile(ConfigurationPath, encryptedConfiguration);
        }

        public CurrentUserConfiguration OpenConfiguration(string password)
        {
            string encryptedConfigurationContent = FileModification.ReadFile(ConfigurationPath);
            if (encryptedConfigurationContent != "")
            {
                string configurationContent = EncryptingHelper.Decrypt(encryptedConfigurationContent, password);
                return JsonConvert.DeserializeObject<CurrentUserConfiguration>(configurationContent);
            }

            return null;
        }
    }
}
