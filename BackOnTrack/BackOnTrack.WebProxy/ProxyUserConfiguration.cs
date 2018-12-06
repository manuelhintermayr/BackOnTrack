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

        public void LoadCurrentUserConfiguration()
        {

        }

        public void ApplyUserConfiguration()
        {

        }

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
