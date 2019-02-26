using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.SharedResources.Infrastructure.Helpers;
using BackOnTrack.SharedResources.Models;
using BackOnTrack.WebProxy.Exceptions;
using Newtonsoft.Json;

namespace BackOnTrack.WebProxy
{
    public class ProxyUserConfiguration
    {
        private const string _configurationPassword = "BackOnTrackProxy";
        private CurrentUserConfiguration _currentConfiguration;
        private List<string> _listOfRegexBlockedSites;
        private List<RedirectEntry> _listOfRedirectSites;
        private List<string> _listOfBlockedSites;
        private List<RedirectEntry> _listOfRegexRedirectSites;
        private string _programSettingsPath;

        public ProxyUserConfiguration(string programSettingsPath)
        {
            _programSettingsPath = programSettingsPath;
            _listOfBlockedSites = new List<string>();
            _listOfRegexBlockedSites = new List<string>();
            _listOfRedirectSites = new List<RedirectEntry>();
            _listOfRegexRedirectSites = new List<RedirectEntry>();
        }

        #region Getter

        public List<string> GetListOfBlockedSites()
        {
            return _listOfBlockedSites;
        }

        public List<string> GetListOfRegexBlockedSites()
        {
            return _listOfRegexBlockedSites;
        }

        public List<RedirectEntry> GetListOfRedirectSites()
        {
            return _listOfRedirectSites;
        }

        public List<RedirectEntry> GetListOfRegexRedirectSites()
        {
            return _listOfRegexRedirectSites;
        }

        public string GetProxyUserConfigurationPath()
        {
            return $"{_programSettingsPath}\\.backOnTrack\\proxy.profiles";
        }

        #endregion
        #region Loading user configuration from file system

        public void LoadCurrentUserConfiguration()
        {
            if (!FileModification.FileExists(GetProxyUserConfigurationPath()))
            {
                throw new WebProxyNoProfilesFileException($"WebProxy file \"{GetProxyUserConfigurationPath()}\" does not exist.");
            }
            else
            {
                string encryptedConfigurationContent = FileModification.ReadFile(GetProxyUserConfigurationPath());
                bool fileIsBroken = false;
                if (encryptedConfigurationContent != "")
                {
                    try
                    {
                        string configurationContent =
                            EncryptingHelper.Decrypt(encryptedConfigurationContent, _configurationPassword);
                        var configuration =
                            JsonConvert.DeserializeObject<CurrentUserConfiguration>(configurationContent);
                        ApplyUserConfiguration(configuration, false);
                    }
                    catch (Exception)
                    {
                        fileIsBroken = true;
                    }

                }
                else
                {
                    fileIsBroken = true;
                }

                if (fileIsBroken)
                {
                    throw new WebProxyBrokenProfileConfigurationException($"WebProxy file \"{GetProxyUserConfigurationPath()}\" is broken.");
                }
            }
        }

        #endregion
        #region Applying specific user configuration

        public void ApplyUserConfiguration(CurrentUserConfiguration userConfiguration, bool saveConfiguration = true)
        {
            _currentConfiguration = userConfiguration;           
            _listOfRedirectSites.Clear();
            _listOfRegexRedirectSites.Clear();
            FillListOfBlockedSites();
            FillListOfRegexBlockedSites();
            FillListOfRedirectSites();
            FillListOfRegexRedirectSites();

            if (saveConfiguration)
            {
                SaveUserConfiguration(userConfiguration);
            }
        }

        private void FillListOfBlockedSites()
        {
            _listOfBlockedSites.Clear();

            var listOfBlockedSites = (
                from profile in _currentConfiguration.ProfileList
                from entry in profile.EntryList
                where
                    profile.ProfileIsEnabled &&
                    entry.EntryType == EntryType.Block &&
                    entry.IsEnabled &&
                    entry.ProxyBlockingIsEnabled
                select entry.Url
            ).ToList();

            foreach (var item in listOfBlockedSites)
            {
                _listOfBlockedSites.Add(item);
            }
        }

        private void FillListOfRegexBlockedSites()
        {
            _listOfRegexBlockedSites.Clear();

            var listOfRegexBlockedSites = (
                from profile in _currentConfiguration.ProfileList
                from entry in profile.EntryList
                where
                    profile.ProfileIsEnabled &&
                    entry.EntryType == EntryType.RegexBlock &&
                    entry.IsEnabled &&
                    entry.ProxyBlockingIsEnabled
                select entry.Url
            ).ToList();

            foreach (var item in listOfRegexBlockedSites)
            {
                _listOfRegexBlockedSites.Add(item);
            }
        }

        private void FillListOfRedirectSites()
        {
            _listOfRedirectSites.Clear();

            List<RedirectEntry> listOfRedirectSites = (
                from profile in _currentConfiguration.ProfileList
                from entry in profile.EntryList
                where
                    profile.ProfileIsEnabled &&
                    entry.EntryType == EntryType.Redirect &&
                    entry.IsEnabled &&
                    entry.ProxyBlockingIsEnabled
                select new RedirectEntry()
                {
                    AddressRedirectFrom = entry.Url,
                    AddressRedirectTo = entry.RedirectUrl
                }
            ).ToList();

            foreach (var item in listOfRedirectSites)
            {
                _listOfRedirectSites.Add(item);
            }
        }

        private void FillListOfRegexRedirectSites()
        {
            _listOfRegexRedirectSites.Clear();

            List<RedirectEntry> listOfRegexRedirectSites = (
                from profile in _currentConfiguration.ProfileList
                from entry in profile.EntryList
                where
                    profile.ProfileIsEnabled &&
                    entry.EntryType == EntryType.RegexRedirect &&
                    entry.IsEnabled &&
                    entry.ProxyBlockingIsEnabled
                select new RedirectEntry()
                {
                    AddressRedirectFrom = entry.Url,
                    AddressRedirectTo = entry.RedirectUrl
                }
            ).ToList();

            foreach (var item in listOfRegexRedirectSites)
            {
                _listOfRegexRedirectSites.Add(item);
            }
        }

        #endregion

        private void SaveUserConfiguration(CurrentUserConfiguration userConfiguration)
        {
            var jsonConfiguration = JsonConvert.SerializeObject(userConfiguration);
            string encryptedConfiguration = EncryptingHelper.Encrypt(jsonConfiguration, _configurationPassword);
            FileModification.CreateFolderIfNotExists($"{_programSettingsPath}\\.backOnTrack");

            FileModification.WriteFile(GetProxyUserConfigurationPath(), encryptedConfiguration);
        }

        public void CreateEmptyProfileConfigurationIfNotExists()
        {
            if (!FileModification.FileExists(GetProxyUserConfigurationPath()))
            {
                SaveUserConfiguration(new CurrentUserConfiguration());
            }
        }
    }

    public class RedirectEntry
    {
        public string AddressRedirectFrom { get; set; }
        public string AddressRedirectTo { get; set; }
    }
}
