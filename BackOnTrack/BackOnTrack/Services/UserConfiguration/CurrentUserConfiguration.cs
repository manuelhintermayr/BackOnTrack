using System.Collections.Generic;
using System.ComponentModel;

namespace BackOnTrack.Services.UserConfiguration
{
    public class CurrentUserConfiguration : INotifyPropertyChanged
    {
        private List<Profile> _profileList;
        public List<Profile> ProfileList { 
            get { return _profileList; }
            set
            {
                _profileList = value;
                OnPropertyChanged("ProfileList");
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
        public bool PreferableBlockingOnSystemLevel;
        public bool PreferableBlockingOnProxyLevel;

        public static Profile CreateProfile(string profileName, bool blockOnSystemLevel, bool blockOnProxyLevel)
        {
            return new Profile() { ProfileName = profileName, EntryList = new List<Entry>(), ProfileIsEnabled = true, PreferableBlockingOnSystemLevel = blockOnSystemLevel, PreferableBlockingOnProxyLevel = blockOnProxyLevel};
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


        public static Entry CreateBlockEntry(string url, bool systemLevelBlockingIsEnabled, bool proxyBlockingIsEnabled,  bool isEnabled = true)
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
