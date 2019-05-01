using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.SharedResources.Models;
using BackOnTrack.SharedResources.Tests.Base;
using BackOnTrack.Tests.Base;
using BackOnTrack.UI.MainView.Pages.Profiles;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.Tests
{
    [TestClass]
    public class AddProfileTests : RunningApplicationTestBase
    {
        [TestMethod]
        [TestProperty("Number", "18")]
        [TestProperty("Type", "Integration")]
        public void CheckIfProfileNameIsAlreadyUsed() 
        {
            //Arrange
            Profile profile1 = new Profile() { ProfileName = "Manuelweb", EntryList = new List<Entry>() };
            Profile profile2 = new Profile() { ProfileName = "Financeviewer", EntryList = new List<Entry>() };
            Profile profile3 = new Profile() { ProfileName = "Manuelweb", EntryList = new List<Entry>() };
            SetupUnlockedRunningApplication();

            //Act
            bool firstProfileNameIsAlreadyUsed = AddProfile.CheckIfProfileNameIsAlreadyUsed(profile1.ProfileName);
            Application.UI.MainView.UserConfiguration.ProfileList.Add(profile1);
            bool secondProfileNameIsAlreadyUsed = AddProfile.CheckIfProfileNameIsAlreadyUsed(profile2.ProfileName);
            Application.UI.MainView.UserConfiguration.ProfileList.Add(profile2);
            bool thirdProfileNameIsAlreadyUsed = AddProfile.CheckIfProfileNameIsAlreadyUsed(profile3.ProfileName);

            //Assert
            firstProfileNameIsAlreadyUsed.Should().BeFalse();
            secondProfileNameIsAlreadyUsed.Should().BeFalse();
            thirdProfileNameIsAlreadyUsed.Should().BeTrue();
        }
    }
}
