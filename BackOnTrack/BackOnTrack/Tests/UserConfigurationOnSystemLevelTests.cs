using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.Services.UserConfiguration;
using BackOnTrack.SharedResources.Infrastructure.Helpers;
using BackOnTrack.SharedResources.Models;
using BackOnTrack.SharedResources.Tests.Base;
using BackOnTrack.Tests.Base;
using BackOnTrack.UI.MainView.Pages.Profiles;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.Tests
{
    [TestClass]
    public class UserConfigurationOnSystemLevelTests : RunningApplicationTestBase
    {
        private UserConfigurationOnSystemLevel _ucosl;
        private SpecificProfileView _profileView;
        private Entry _activatedEntry;
        private Entry _disabledEntry;
        private void DoSetupWithUnlockingAndBasicHostFileWithUcosl()
        {
            DoSetupWithUnlockingAndBasicHostFile();
            _ucosl = new UserConfigurationOnSystemLevel();
        }

        private void CreateProfileViewWithAddedEnAndDisabledEntry()
        {
            _profileView = CreateTestableProfileView();
            _activatedEntry = Entry.CreateBlockEntry("manuelweb.at", true, true);
            _disabledEntry = Entry.CreateBlockEntry("www.manuelweb.at", true, true, false);
            _profileView.AddNewEntry(_activatedEntry);
            _profileView.AddNewEntry(_disabledEntry);
        }


        [TestMethod]
        [TestProperty("Number", "28")]
        [TestProperty("Type", "Integration")]
        public void LoadingEntriesFromNoExistingHostFileShouldPutZeroEntriesIntoTheHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUcosl();

            //Act
            _ucosl.AddAllLinesFromHostFileIntoEntryList();

            //Arrange
            _ucosl.GetHostEntries().Count.Should().Be(0);
        }

        [TestMethod]
        [TestProperty("Number", "6")]
        [TestProperty("Type", "Integration")]
        public void LoadingEntriesFromEmptyHostFileShouldPutZeroEntriesIntoTheHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUcosl();
            FileModification.WriteFile(NewHostFileTwoLocation, "");

            //Act
            _ucosl.AddAllLinesFromHostFileIntoEntryList();

            //Arrange
            _ucosl.GetHostEntries().Count.Should().Be(0);
        }

        [TestMethod]
        [TestProperty("Number", "39")]
        [TestProperty("Type", "Integration")]
        public void EntryFromHostFileShouldHaveBeenPutCorrectlyIntoTheHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUcosl();
            CreateHostFileWithSampleContent();

            //Act
            _ucosl.AddAllLinesFromHostFileIntoEntryList();
            var resultList = _ucosl.GetHostEntries();

            //Arrange
            resultList.Count.Should().Be(2);
            resultList[0].Content.Should().Be("#Test");
            resultList[1].Content.Should().Be("127.0.0.1 facebook.com");
        }

        [TestMethod]
        [TestProperty("Number", "9")]
        [TestProperty("Type", "Integration")]
        public void EntryFromProfileWhereAddedCorrectlyToHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUcosl();
            CreateHostFileWithSampleContent();
            _ucosl.AddAllLinesFromHostFileIntoEntryList();
            CreateProfileViewWithAddedEnAndDisabledEntry();

            //Act
            _ucosl.AddMissingEntriesIntoEntryList(Application.UI.MainView.UserConfiguration);
            var resultList = _ucosl.GetHostEntries();

            //Assert
            resultList.Count.Should().Be(3);
            resultList[2].Content.Should().Be("127.0.0.1  manuelweb.at #BackOnTrackEntry");
        }

        [TestMethod]
        [TestProperty("Number", "35")]
        [TestProperty("Type", "Integration")]
        public void EntriesFromDisabledProfileShouldNotBeAddedToHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUcosl();
            CreateHostFileWithSampleContent();
            _ucosl.AddAllLinesFromHostFileIntoEntryList();
            CreateProfileViewWithAddedEnAndDisabledEntry();
            var userConfiguration = Application.UI.MainView.UserConfiguration;

            //Act
            userConfiguration.ProfileList[0].ProfileIsEnabled = false;
            _ucosl.AddMissingEntriesIntoEntryList(Application.UI.MainView.UserConfiguration);
            var resultList = _ucosl.GetHostEntries();

            //Assert
            resultList.Count.Should().Be(2);
        }

        [TestMethod]
        [TestProperty("Number", "34")]
        [TestProperty("Type", "Integration")]
        public void NotActiveBackOnTrackEntriesShouldGetRemovedFromHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUcosl();
            FileModification.WriteFile(NewHostFileTwoLocation, "#Test" + Environment.NewLine + "127.0.0.1  manuelweb.at #BackOnTrackEntry");
            var hostEntries = _ucosl.GetHostEntries();
            _ucosl.AddAllLinesFromHostFileIntoEntryList();
            _ucosl.AddMissingEntriesIntoEntryList(Application.UI.MainView.UserConfiguration);

            //Act
            int hostEntriesBeforeRemovingNotActiveOnes = hostEntries.Count;
            _ucosl.RemoveNotActiveEntriesFromEntryList(Application.UI.MainView.UserConfiguration);
            int hostEntriesAfterRemovingNotActiveOnes = hostEntries.Count;

            //Assert
            hostEntriesBeforeRemovingNotActiveOnes.Should().Be(2);
            hostEntriesAfterRemovingNotActiveOnes.Should().Be(1);
        }

        [TestMethod]
        [TestProperty("Number", "26")]
        [TestProperty("Type", "Integration")]
        public void AlreadyExistingAndStillActiveEntriesShouldNotGetRemovedFromHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUcosl();
            FileModification.WriteFile(NewHostFileTwoLocation, "#Test" + Environment.NewLine + "127.0.0.1  manuelweb.at #BackOnTrackEntry");
            var hostEntries = _ucosl.GetHostEntries();
            _ucosl.AddAllLinesFromHostFileIntoEntryList();
            CreateProfileViewWithAddedEnAndDisabledEntry();
            _ucosl.AddMissingEntriesIntoEntryList(Application.UI.MainView.UserConfiguration);

            //Act
            int hostEntriesBeforeRemovingNotActiveOnes = hostEntries.Count;
            _ucosl.RemoveNotActiveEntriesFromEntryList(Application.UI.MainView.UserConfiguration);
            int hostEntriesAfterRemovingNotActiveOnes = hostEntries.Count;

            //Assert
            hostEntriesBeforeRemovingNotActiveOnes.Should().Be(2);
            hostEntriesAfterRemovingNotActiveOnes.Should().Be(2);
        }


        [TestMethod]
        [TestProperty("Number", "36")]
        [TestProperty("Type", "Integration")]
        public void GetCorrectListOfActiveBlockEntries()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUcosl();
            CreateHostFileWithSampleContent();
            _ucosl.AddAllLinesFromHostFileIntoEntryList();
            CreateProfileViewWithAddedEnAndDisabledEntry();
            var userConfiguration = Application.UI.MainView.UserConfiguration;

            //Act
            var result = UserConfigurationOnSystemLevel.GetListOfActiveBlockEntries(userConfiguration);

            //Assert
            result.Count.Should().Be(1);
            result[0].Should().Be(_activatedEntry.Url);
        }

        [TestMethod]
        [TestProperty("Number", "40")]
        [TestProperty("Type", "Integration")]
        public void ListOfActiveBlockEntriesDoesNotShowEntriesWithDisabledProfiles()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUcosl();
            CreateHostFileWithSampleContent();
            _ucosl.AddAllLinesFromHostFileIntoEntryList();
            CreateProfileViewWithAddedEnAndDisabledEntry();         
            var userConfiguration = Application.UI.MainView.UserConfiguration;

            //Act
            userConfiguration.ProfileList[0].ProfileIsEnabled = false;
            var result = UserConfigurationOnSystemLevel.GetListOfActiveBlockEntries(userConfiguration);

            //Assert
            result.Count.Should().Be(0);
        }
    }
}
