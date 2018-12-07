using System.Collections.Generic;
using System.ComponentModel;

namespace BackOnTrack.SharedResources.Models
{
    public class CurrentUserConfiguration : INotifyPropertyChanged
    {
        private List<Profile> _profileList;
        public List<Profile> ProfileList
        {
            get { return _profileList; }
            set
            {
                _profileList = value;
                OnPropertyChanged("ProfileList");
            }
        }

        public CurrentUserConfiguration()
        {
            if (ProfileList == null)
            {
                ProfileList = new List<Profile>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }

    public class Profile : INotifyPropertyChanged
    {
        public string ProfileName { get; set; }
        private bool _profileIsEnabled;
        public bool ProfileIsEnabled
        {
            get { return _profileIsEnabled; }
            set
            {
                _profileIsEnabled = value;
                OnPropertyChanged("ProfileIsEnabled");
            }
        }
        public List<Entry> EntryList;
        public bool PreferableBlockingOnSystemLevel; //this means that new entries over the UI will be created with this value 
        public bool PreferableBlockingOnProxyLevel; //this means that new entries over the UI will be created with this value

        public static Profile CreateProfile(string profileName, bool preferableBlockOnSystemLevel, bool preferableBlockOnProxyLevel)
        {
            return new Profile() { ProfileName = profileName, EntryList = new List<Entry>(), ProfileIsEnabled = true, PreferableBlockingOnSystemLevel = preferableBlockOnSystemLevel, PreferableBlockingOnProxyLevel = preferableBlockOnProxyLevel };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }

    public class Entry
    {
        public string Url { get; set; }
        public EntryType EntryType { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsEnabled { get; set; }
        public bool SystemLevelBlockingIsEnabled { get; set; }
        public bool ProxyBlockingIsEnabled { get; set; }


        public static Entry CreateBlockEntry(string url, bool systemLevelBlockingIsEnabled, bool proxyBlockingIsEnabled, bool isEnabled = true)
        {
            return new Entry()
            {
                Url = url,
                IsEnabled = isEnabled,
                SystemLevelBlockingIsEnabled = systemLevelBlockingIsEnabled,
                ProxyBlockingIsEnabled = proxyBlockingIsEnabled,
                EntryType = EntryType.Block
            };
        }
        public static Entry CreateRedirectEntry(string url, string redirectUrl, bool systemLevelBlockingIsEnabled, bool proxyBlockingIsEnabled, bool isEnabled = true)
        {
            return new Entry()
            {
                Url = url,
                RedirectUrl = redirectUrl,
                IsEnabled = isEnabled,
                SystemLevelBlockingIsEnabled = systemLevelBlockingIsEnabled,
                ProxyBlockingIsEnabled = proxyBlockingIsEnabled,
                EntryType = EntryType.Redirect
            };
        }
    }

    public enum EntryType { Block, Redirect };

}
