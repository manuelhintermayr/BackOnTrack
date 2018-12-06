using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.SharedResources.Infrastructure.Helpers;
using BackOnTrack.SharedResources.Models;
using BackOnTrack.WebProxy.Exceptions;

namespace BackOnTrack.WebProxy
{
    public class ProxyUserConfiguration
    {
        private CurrentUserConfiguration _currentConfiguration;
        private List<string> _listOfBlockedSites;
        private List<RedirectEntry> _listOfRedirectSites;

        public ProxyUserConfiguration()
        {
            _listOfBlockedSites = new List<string>();
            _listOfRedirectSites = new List<RedirectEntry>();
        }

        #region Getter

        public List<string> GetListOfBlockedSites()
        {
            return _listOfBlockedSites;
        }

        public List<RedirectEntry> GetListOfRedirectSites()
        {
            return _listOfRedirectSites;
        }

        public string GetProxyUserConfigurationPath()
        {
            return $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\.backOnTrack\\proxy.profiles";
        }

        #endregion
        #region Loading user configuration from file system

        public void LoadCurrentUserConfiguration()
        {
            if (!FileModification.FileExists(GetProxyUserConfigurationPath()))
            {
                throw new WebProxyNoProfilesFileException($"WebProxy file \"{GetProxyUserConfigurationPath()}\" does not exist.");
            }
            //...
            //ApplyUserConfiguration(false);
        }

        #endregion
        #region Applying specific user configuration

        public void ApplyUserConfiguration(CurrentUserConfiguration userConfiguration, bool saveConfiguration = true)
        {
            _currentConfiguration = userConfiguration;           
            _listOfRedirectSites.Clear();
            FillListOfBlockedSites();
            FillListOfRedirectSites();

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

        #endregion

        private void SaveUserConfiguration(CurrentUserConfiguration userConfiguration)
        {

        }

    }

    public class RedirectEntry
    {
        public string AddressRedirectFrom { get; set; }
        public string AddressRedirectTo { get; set; }
    }
}
