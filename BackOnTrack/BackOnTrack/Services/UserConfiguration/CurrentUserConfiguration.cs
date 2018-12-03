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

        public static Entry CreateEntry(string content, EntryType entryType, bool isEnabled = true)
        {
            return new Entry() { Url = content, IsEnabled = isEnabled, EntryType = entryType};
        }
    }
    public enum EntryType { Block, Redirect };
}
