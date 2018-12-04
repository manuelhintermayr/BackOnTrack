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
        public bool IsEnabled { get; set; }

        public EntryType EntryType { get; set; }

        public static Entry CreateBlockEntry(string url, bool isEnabled = true)
        {
            return new Entry() { Url = url, IsEnabled = isEnabled, EntryType = EntryType.Block};
        }
        public static RedirectEntry CreateRedirectEntry(string url, string redirectUrl, bool isEnabled = true)
        {
            return new RedirectEntry() { Url = url, RedirectUrl = redirectUrl, IsEnabled = isEnabled, EntryType = EntryType.Redirect };
        }
    }

    public class RedirectEntry : Entry
    {
        public string RedirectUrl { get; set; }
    }

    public enum EntryType { Block, Redirect };
}
