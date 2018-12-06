using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.SharedResources.Models;

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

        #region Lists getter

        public List<string> GetListOfBlockedSites()
        {
            return _listOfBlockedSites;
        }

        public List<RedirectEntry> GetListOfRedirectSites()
        {
            return _listOfRedirectSites;
        }

        #endregion
        #region Loading user configuration from file system

        public void LoadCurrentUserConfiguration()
        {
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






        private void SaveUserConfiguration()
        {

        }

    }

    public class RedirectEntry
    {
        public string AddressRedirectFrom { get; set; }
        public string AddressRedirectTo { get; set; }
    }
}
