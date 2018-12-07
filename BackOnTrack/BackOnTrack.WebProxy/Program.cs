using System;
using System.Collections.Generic;
using BackOnTrack.SharedResources.Models;

namespace BackOnTrack.WebProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new LocalWebProxy();
            CurrentUserConfiguration newConfiguration = new CurrentUserConfiguration();
            Profile newProfile = Profile.CreateProfile("Google", false, true);
            newProfile.EntryList.Add(Entry.CreateBlockEntry("google.com", false, true));

            proxy.ApplyUserConfigurationOnProxy(newConfiguration, false);
            proxy.StartProxy();
            Console.Read();
            proxy.QuitProxy();
        }

    }
}
