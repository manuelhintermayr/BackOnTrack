using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.SharedResources.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.WebProxy.Tests
{
    [TestClass]
    public class ProxyUserConfigurationTests
    {
        private ProxyUserConfiguration proxyUserConfiguration;
        private CurrentUserConfiguration userConfiguration;
        private Profile newProfile;

        public ProxyUserConfigurationTests()
        {
            proxyUserConfiguration = new ProxyUserConfiguration();
            userConfiguration = new CurrentUserConfiguration();
            newProfile = Profile.CreateProfile("Google", false, true);
        }

        [TestMethod]
        public void ListsShouldBeEmptyAfterInitialisation()
        {
            //Arrange & Act
            proxyUserConfiguration = new ProxyUserConfiguration();

            //Assert
            proxyUserConfiguration.GetListOfBlockedSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRegexBlockedSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRedirectSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRegexRedirectSites().Count.Should().Be(0);
        }

        [TestMethod]
        public void ListsShouldBeEmptyAfterInsertingEmptyUserConfiguration()
        {
            //Arrange
            userConfiguration = new CurrentUserConfiguration();

            //Act
            proxyUserConfiguration.ApplyUserConfiguration(userConfiguration);

            //Assert
            proxyUserConfiguration.GetListOfBlockedSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRegexBlockedSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRedirectSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRegexRedirectSites().Count.Should().Be(0);
        }

        [TestMethod]
        public void BlockedSitesInProfileShouldBeInNormalBlockList()
        {
            //Arrange
            var blockEntry1 = Entry.CreateBlockEntry("google.com", false, true);
            var blockEntry2 = Entry.CreateBlockEntry("m.google.com", false, true);
            var blockEntry3 = Entry.CreateBlockEntry("touch.google.com", false, true);

            //Act
            newProfile.EntryList.Add(blockEntry1);
            newProfile.EntryList.Add(blockEntry2);
            newProfile.EntryList.Add(blockEntry3);
            AddProfileAndApplyConfiguration();

            //Assert
            proxyUserConfiguration.GetListOfBlockedSites().Count.Should().Be(3);
            proxyUserConfiguration.GetListOfBlockedSites()[0].Should().Be("google.com");
            proxyUserConfiguration.GetListOfBlockedSites()[1].Should().Be("m.google.com");
            proxyUserConfiguration.GetListOfBlockedSites()[2].Should().Be("touch.google.com");

            proxyUserConfiguration.GetListOfRegexBlockedSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRedirectSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRegexRedirectSites().Count.Should().Be(0);
        }

        [TestMethod]
        public void BlockedSitesInProfileShouldBeInRegexBlockList()
        {
            //Arrange
            var blockEntry1 = Entry.CreateRegexBlockEntry("google.com", true);
            var blockEntry2 = Entry.CreateRegexBlockEntry("m.google.com", true);
            var blockEntry3 = Entry.CreateRegexBlockEntry("touch.google.com", true);

            //Act
            newProfile.EntryList.Add(blockEntry1);
            newProfile.EntryList.Add(blockEntry2);
            newProfile.EntryList.Add(blockEntry3);
            AddProfileAndApplyConfiguration();

            //Assert
            proxyUserConfiguration.GetListOfRegexBlockedSites().Count.Should().Be(3);
            proxyUserConfiguration.GetListOfRegexBlockedSites()[0].Should().Be("google.com");
            proxyUserConfiguration.GetListOfRegexBlockedSites()[1].Should().Be("m.google.com");
            proxyUserConfiguration.GetListOfRegexBlockedSites()[2].Should().Be("touch.google.com");

            proxyUserConfiguration.GetListOfBlockedSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRedirectSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRegexRedirectSites().Count.Should().Be(0);
        }

        [TestMethod]
        public void RedirectSitesInProfileShouldBeInNormalRedirectList()
        {
            //Arrange
            var redirectEntry1 = Entry.CreateRedirectEntry("google.com", "wikipedia.org", false, true);
            var redirectEntry2 = Entry.CreateRedirectEntry("m.google.com", "wikipedia.org", false, true);
            var redirectEntry3 = Entry.CreateRedirectEntry("touch.google.com", "wikipedia.org", false, true);

            //Act
            newProfile.EntryList.Add(redirectEntry1);
            newProfile.EntryList.Add(redirectEntry2);
            newProfile.EntryList.Add(redirectEntry3);
            AddProfileAndApplyConfiguration();

            //Assert
            proxyUserConfiguration.GetListOfRedirectSites().Count.Should().Be(3);
            proxyUserConfiguration.GetListOfRedirectSites()[0].AddressRedirectTo.Should().Be("wikipedia.org");
            
            proxyUserConfiguration.GetListOfBlockedSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRegexBlockedSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRegexRedirectSites().Count.Should().Be(0);
        }

        [TestMethod]
        public void RedirectSitesInProfileShouldBeInRegexRedirectList()
        {
            //Arrange
            var redirectEntry1 = Entry.CreateRegexRedirectEntry("google.com", "wikipedia.org", true, true);
            var redirectEntry2 = Entry.CreateRegexRedirectEntry("m.google.com", "wikipedia.org", true, true);
            var redirectEntry3 = Entry.CreateRegexRedirectEntry("touch.google.com", "wikipedia.org", true, true);

            //Act
            newProfile.EntryList.Add(redirectEntry1);
            newProfile.EntryList.Add(redirectEntry2);
            newProfile.EntryList.Add(redirectEntry3);
            AddProfileAndApplyConfiguration();

            //Assert
            proxyUserConfiguration.GetListOfRegexRedirectSites().Count.Should().Be(3);
            proxyUserConfiguration.GetListOfRegexRedirectSites()[0].AddressRedirectTo.Should().Be("wikipedia.org");

            proxyUserConfiguration.GetListOfBlockedSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRegexBlockedSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRedirectSites().Count.Should().Be(0);
        }

        [TestMethod]
        public void DoNotAddDisabledBlockEntries()
        {
            //Arrange
            var enabledBlockEntry = Entry.CreateBlockEntry("google.com", false, true);
            var disabledBlockEntry = Entry.CreateBlockEntry("m.google.com", false, false);

            //Act
            newProfile.EntryList.Add(enabledBlockEntry);
            newProfile.EntryList.Add(disabledBlockEntry);
            AddProfileAndApplyConfiguration();

            //Assert
            proxyUserConfiguration.GetListOfBlockedSites().Count.Should().NotBe(2);
            proxyUserConfiguration.GetListOfBlockedSites().Count.Should().Be(1);
        }

        [TestMethod]
        public void DoNotAddDisabledRegexBlockEntries()
        {
            //Arrange
            var enabledBlockEntry = Entry.CreateRegexBlockEntry("google.com", true, true);
            var disabledBlockEntry = Entry.CreateRegexBlockEntry("m.google.com", false, false);

            //Act
            newProfile.EntryList.Add(enabledBlockEntry);
            newProfile.EntryList.Add(disabledBlockEntry);
            AddProfileAndApplyConfiguration();

            //Assert
            proxyUserConfiguration.GetListOfRegexBlockedSites().Count.Should().NotBe(2);
            proxyUserConfiguration.GetListOfRegexBlockedSites().Count.Should().Be(1);
        }


        [TestMethod]
        public void DoNotAddDisabledRedirectEntries()
        {
            //Arrange
            var enabledRedirectEntry = Entry.CreateRedirectEntry("google.com", "wikipedia.org", false, true);
            var disableRedirectEntryEntry = Entry.CreateRedirectEntry("m.google.com", "wikipedia.org", false, false);

            //Act
            newProfile.EntryList.Add(enabledRedirectEntry);
            newProfile.EntryList.Add(disableRedirectEntryEntry);
            AddProfileAndApplyConfiguration();

            //Assert
            proxyUserConfiguration.GetListOfRedirectSites().Count.Should().NotBe(2);
            proxyUserConfiguration.GetListOfRedirectSites().Count.Should().Be(1);
        }

        [TestMethod]
        public void DoNotAddDisabledRegexRedirectEntries()
        {
            //Arrange
            var enabledRedirectEntry = Entry.CreateRegexRedirectEntry("google.com", "wikipedia.org", true, true);
            var disableRedirectEntryEntry = Entry.CreateRegexRedirectEntry("m.google.com", "wikipedia.org", false, false);

            //Act
            newProfile.EntryList.Add(enabledRedirectEntry);
            newProfile.EntryList.Add(disableRedirectEntryEntry);
            AddProfileAndApplyConfiguration();

            //Assert
            proxyUserConfiguration.GetListOfRegexRedirectSites().Count.Should().NotBe(2);
            proxyUserConfiguration.GetListOfRegexRedirectSites().Count.Should().Be(1);
        }

        [TestMethod]
        public void ThereShouldBeNoEntriesForDisabledProfile()
        {
            //Arrange
            newProfile.ProfileIsEnabled = false; //disable profile
            var enabledBlockEntry = Entry.CreateBlockEntry("google.com", false, true);
            var enabledRedirectEntry = Entry.CreateRedirectEntry("m.google.com", "wikipedia.org", false, true);
            var enabledRegexBlockEntry = Entry.CreateRegexBlockEntry("google.com", true);
            var enabledRegexRedirectEntry = Entry.CreateRegexRedirectEntry("m.google.com", "wikipedia.org", true);

            //Act
            newProfile.EntryList.Add(enabledBlockEntry);
            newProfile.EntryList.Add(enabledRedirectEntry);
            newProfile.EntryList.Add(enabledRegexBlockEntry);
            newProfile.EntryList.Add(enabledRegexRedirectEntry);
            AddProfileAndApplyConfiguration();

            //Assert
            proxyUserConfiguration.GetListOfBlockedSites().Count.Should().NotBe(1);
            proxyUserConfiguration.GetListOfBlockedSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRegexBlockedSites().Count.Should().NotBe(1);
            proxyUserConfiguration.GetListOfRegexBlockedSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRedirectSites().Count.Should().NotBe(1);
            proxyUserConfiguration.GetListOfRedirectSites().Count.Should().Be(0);
            proxyUserConfiguration.GetListOfRegexRedirectSites().Count.Should().NotBe(1);
            proxyUserConfiguration.GetListOfRegexRedirectSites().Count.Should().Be(0);
        }

        private void AddProfileAndApplyConfiguration()
        {
            userConfiguration.ProfileList.Add(newProfile);
            proxyUserConfiguration.ApplyUserConfiguration(userConfiguration);
        }
    }
}
