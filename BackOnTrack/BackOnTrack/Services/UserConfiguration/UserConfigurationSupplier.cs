using System;
using System.Collections.Generic;
using System.IO;
using BackOnTrack.Infrastructure.Exceptions;
using BackOnTrack.Infrastructure.Helpers;
using Newtonsoft.Json;

namespace BackOnTrack.Services.UserConfiguration
{
    public class UserConfigurationSupplier
    {
        public string ConfigurationPath;
        private UserConfigurationOnSystemLevel systemLevel;

        public UserConfigurationSupplier()
        {
            ConfigurationPath = 
                $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\.backOnTrack\\profiles.settings";
            //todo: ^make configurable for testing
            systemLevel = new UserConfigurationOnSystemLevel();
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

            SaveConfiguration(configuration, password);
        }

        public void SaveConfiguration(CurrentUserConfiguration configuration, string password)
        {
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

        #region UserConfiguration on System Level

        public void ApplyUserConfigurationOnSystemLevel(CurrentUserConfiguration newConfiguration)
        {
            systemLevel.ApplyingConfiguration(newConfiguration);   
        }

        #endregion

        #region UserConfiguration on Proxy Level

        public void ApplyUserConfigurationOnProxy(CurrentUserConfiguration newConfiguration)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
