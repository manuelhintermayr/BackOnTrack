using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.SharedResources.Infrastructure.Helpers;
using BackOnTrack.SharedResources.Models;
using BackOnTrack.SharedResources.Tests.Base;
using BackOnTrack.UI.MainView.Pages.Profiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.Tests.Base
{
    [TestClass]
    public class RunningApplicationTestBase : TestBase
    {
        public RunningApplication runningApplication;

        public void SetupUnlockedRunningApplication()
        {
            string password = "admin";
            runningApplication = new RunningApplication(true, TempFolder.Name);
            runningApplication.Services.UserConfiguration.CreateNewConfiguration(password);
            runningApplication.UI.LoginInMainViewWithoutShowing(password);
        }

        public SpecificProfileView CreateTestableProfileView()
        {
            Profile profile = new Profile() { ProfileName = "Manuelweb", EntryList = new List<Entry>() };
            runningApplication.UI.MainView.UserConfiguration.ProfileList.Add(profile);

            SpecificProfileView profileView = new SpecificProfileView("Manuelweb");
            return profileView;
        }
    }
}
