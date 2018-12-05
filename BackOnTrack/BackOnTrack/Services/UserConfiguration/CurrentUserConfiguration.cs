using System.Collections.Generic;

namespace BackOnTrack.Services.UserConfiguration
{
    public class CurrentUserConfiguration
    {
        public List<Profile> ProfileList; 
    }

    public class Profile
    {
        public string ProfileName { get; set; }
        public List<Entry> EntryList;
        public bool PreferableBlockingOnSystemLevel;
        public bool PreferableBlockingOnProxyLevel;

        public static Profile CreateProfile(string profileName, bool blockOnSystemLevel, bool blockOnProxyLevel)
        {
            return new Profile() { ProfileName = profileName, EntryList = new List<Entry>(), PreferableBlockingOnSystemLevel = blockOnSystemLevel, PreferableBlockingOnProxyLevel = blockOnProxyLevel};
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
