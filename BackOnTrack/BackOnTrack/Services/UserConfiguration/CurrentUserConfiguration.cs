using System.Collections.Generic;

namespace BackOnTrack.Services.UserConfiguration
{
    public class CurrentUserConfiguration
    {
        public List<Profile> ProfileList; 
    }

    public class Profile
    {
        public string ProfileName;
        public List<Entry> EntryList;

        public static Profile CreateProfile(string profileName)
        {
            return new Profile() { ProfileName = profileName, EntryList = new List<Entry>() };
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
