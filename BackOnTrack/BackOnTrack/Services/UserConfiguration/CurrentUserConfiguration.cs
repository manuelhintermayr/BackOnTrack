using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOnTrack.Services.UserConfiguration
{
    public class CurrentUserConfiguration
    {
        public List<Profile> profileList; 
    }

    public class Profile
    {
        public string profileName;
        public List<Entry> entryList;

        public static Profile CreateProfile(string profileName)
        {
            return new Profile() { profileName = profileName, entryList = new List<Entry>() };
        }
    }

    public class Entry
    {
        public string content;
        public bool isEnabled;

        public static Entry CreateEntry(string content, bool isEnabled = true)
        {
            return new Entry() { content = content, isEnabled = isEnabled };
        }
    }
}
