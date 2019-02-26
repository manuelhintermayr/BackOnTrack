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
        private UserConfigurationOnSystemLevel ucosl;
        public void DoSetupWithUnlockingAndBasicHostFileWithUCOSL()
        {
            DoSetupWithUnlockingAndBasicHostFile();
            ucosl = new UserConfigurationOnSystemLevel();
        }

        [TestMethod]
        public void LoadingEntriesFromNoExistingHostFileShouldPutZeroEntriesIntoTheHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUCOSL();

            //Act
            ucosl.AddAllLinesFromHostFileIntoEntryList();

            //Arrange
            ucosl.GetHostEntries().Count.Should().Be(0);
        }

        [TestMethod]
        public void LoadingEntriesFromEmptyHostFileShouldPutZeroEntriesIntoTheHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUCOSL();
            FileModification.WriteFile(NewHostFileTwoLocation, "");

            //Act
            ucosl.AddAllLinesFromHostFileIntoEntryList();

            //Arrange
            ucosl.GetHostEntries().Count.Should().Be(0);
        }

        [TestMethod]
        public void EntryFromHostFileShouldHaveBeenPutCorrectlyIntoTheHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUCOSL();
            CreateHostFileWithSampleContent();

            //Act
            ucosl.AddAllLinesFromHostFileIntoEntryList();
            var resultList = ucosl.GetHostEntries();

            //Arrange
            resultList.Count.Should().Be(2);
            resultList[0].Content.Should().Be("#Test");
            resultList[1].Content.Should().Be("127.0.0.1 facebook.com");
        }

        [TestMethod]
        public void EntryFromProfileWhereAddedCorrectlyToHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUCOSL();
            CreateHostFileWithSampleContent();
            ucosl.AddAllLinesFromHostFileIntoEntryList();
            SpecificProfileView profileView = CreateTestableProfileView();
            Entry activatedEntry = Entry.CreateBlockEntry("manuelweb.at",true,true);
            Entry disabledEntry = Entry.CreateBlockEntry("www.manuelweb.at", true, true, false);

            //Act
            profileView.AddNewEntry(activatedEntry);
            profileView.AddNewEntry(disabledEntry);
            ucosl.AddMissingEntriesIntoEntryList(Application.UI.MainView.UserConfiguration);
            var resultList = ucosl.GetHostEntries();

            //Assert
            resultList.Count.Should().Be(3);
            resultList[2].Content.Should().Be("127.0.0.1  manuelweb.at #BackOnTrackEntry");
        }

        [TestMethod]
        public void EntriesFromDisabledProfileShouldNotBeAddedToHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUCOSL();
            CreateHostFileWithSampleContent();
            ucosl.AddAllLinesFromHostFileIntoEntryList();
            SpecificProfileView profileView = CreateTestableProfileView();
            Entry activatedEntry = Entry.CreateBlockEntry("manuelweb.at", true, true);            
            profileView.AddNewEntry(activatedEntry);
            var userConfiguration = Application.UI.MainView.UserConfiguration;

            //Act
            userConfiguration.ProfileList[0].ProfileIsEnabled = false;
            ucosl.AddMissingEntriesIntoEntryList(Application.UI.MainView.UserConfiguration);
            var resultList = ucosl.GetHostEntries();

            //Assert
            resultList.Count.Should().Be(2);
        }

        [TestMethod]
        public void NotActiveBackOnTrackEntriesShouldGetRemovedFromHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUCOSL();
            FileModification.WriteFile(NewHostFileTwoLocation, "#Test" + Environment.NewLine + "127.0.0.1  manuelweb.at #BackOnTrackEntry");
            var hostEntries = ucosl.GetHostEntries();
            ucosl.AddAllLinesFromHostFileIntoEntryList();
            ucosl.AddMissingEntriesIntoEntryList(Application.UI.MainView.UserConfiguration);

            //Act
            int hostEntriesBeforeRemovingNotActiveOnes = hostEntries.Count;
            ucosl.RemoveNotActiveEntriesFromEntryList(Application.UI.MainView.UserConfiguration);
            int hostEntriesAfterRemovingNotActiveOnes = hostEntries.Count;

            //Assert
            hostEntriesBeforeRemovingNotActiveOnes.Should().Be(2);
            hostEntriesAfterRemovingNotActiveOnes.Should().Be(1);
        }

        [TestMethod]
        public void AlreadyExistingAndStillActiveEntriesShouldNodGetRemovedFromHostEntryList()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUCOSL();
            FileModification.WriteFile(NewHostFileTwoLocation, "#Test" + Environment.NewLine + "127.0.0.1  manuelweb.at #BackOnTrackEntry");
            var hostEntries = ucosl.GetHostEntries();
            ucosl.AddAllLinesFromHostFileIntoEntryList();
            SpecificProfileView profileView = CreateTestableProfileView();
            Entry activatedEntry = Entry.CreateBlockEntry("manuelweb.at", true, true);
            profileView.AddNewEntry(activatedEntry);
            ucosl.AddMissingEntriesIntoEntryList(Application.UI.MainView.UserConfiguration);

            //Act
            int hostEntriesBeforeRemovingNotActiveOnes = hostEntries.Count;
            ucosl.RemoveNotActiveEntriesFromEntryList(Application.UI.MainView.UserConfiguration);
            int hostEntriesAfterRemovingNotActiveOnes = hostEntries.Count;

            //Assert
            hostEntriesBeforeRemovingNotActiveOnes.Should().Be(2);
            hostEntriesAfterRemovingNotActiveOnes.Should().Be(2);
        }


        [TestMethod]
        public void GetCorrectListOfActiveBlockEntries()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUCOSL();
            CreateHostFileWithSampleContent();
            ucosl.AddAllLinesFromHostFileIntoEntryList();
            SpecificProfileView profileView = CreateTestableProfileView();
            Entry activatedEntry = Entry.CreateBlockEntry("manuelweb.at", true, true);
            profileView.AddNewEntry(activatedEntry);
            var userConfiguration = Application.UI.MainView.UserConfiguration;

            //Act
            var result = UserConfigurationOnSystemLevel.GetListOfActiveBlockEntries(userConfiguration);

            //Assert
            result.Count.Should().Be(1);
            result[0].Should().Be(activatedEntry.Url);
        }

        [TestMethod]
        public void ListOfActiveBlockEntriesDoesNotShowEntriesWithDisabledProfiles()
        {
            //Arrange
            DoSetupWithUnlockingAndBasicHostFileWithUCOSL();
            CreateHostFileWithSampleContent();
            ucosl.AddAllLinesFromHostFileIntoEntryList();
            SpecificProfileView profileView = CreateTestableProfileView();
            Entry activatedEntry = Entry.CreateBlockEntry("manuelweb.at", true, true);
            profileView.AddNewEntry(activatedEntry);
            var userConfiguration = Application.UI.MainView.UserConfiguration;

            //Act
            userConfiguration.ProfileList[0].ProfileIsEnabled = false;
            var result = UserConfigurationOnSystemLevel.GetListOfActiveBlockEntries(userConfiguration);

            //Assert
            result.Count.Should().Be(0);
        }
    }
}
