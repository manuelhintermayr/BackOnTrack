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
        public string Content;
        public bool IsEnabled;

        public static Entry CreateEntry(string content, bool isEnabled = true)
        {
            return new Entry() { Content = content, IsEnabled = isEnabled };
        }
    }
}
