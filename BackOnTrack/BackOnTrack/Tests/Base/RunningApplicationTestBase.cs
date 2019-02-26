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
        public RunningApplication Application;
        public string NewHostFileTwoLocation;

        public void SetupUnlockedRunningApplication()
        {
            string password = "admin";
            Application = new RunningApplication(true, TempFolder.Name);
            Application.Services.UserConfiguration.CreateNewConfiguration(password);
            Application.UI.LoginInMainViewWithoutShowing(password);
        }

        public SpecificProfileView CreateTestableProfileView()
        {
            Profile profile = Profile.CreateProfile("Manuelweb", true, true);
            Application.UI.MainView.UserConfiguration.ProfileList.Add(profile);

            SpecificProfileView profileView = new SpecificProfileView("Manuelweb");
            return profileView;
        }

        public void DoSetupWithUnlockingAndBasicHostFile()
        {
            SetupUnlockedRunningApplication();
            NewHostFileTwoLocation = $"{TempFolder.Name}{@"\hosts"}";
            FileModification.HostFileLocation = NewHostFileTwoLocation;
        }

        public void CreateHostFileWithSampleContent()
        {
            FileModification.WriteFile(NewHostFileTwoLocation, "#Test" + Environment.NewLine + "127.0.0.1 facebook.com");
        }
    }
}
