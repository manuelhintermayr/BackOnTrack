using BackOnTrack.SharedResources.Models;
using BackOnTrack.SharedResources.Tests.Base;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.SharedResources.Tests
{
    [TestClass]
    public class UserConfigurationTests : TestBase
    {
        private Entry entryBlock;
        private Entry entryRegexBlock;
        private Entry entryRedirect;
        private Entry entryRegexRedirect;

        private void CreateAllTypesOfEntries()
        {
            entryBlock = Entry.CreateBlockEntry("", true, true);
            entryRegexBlock = Entry.CreateRegexBlockEntry("", true, true);
            entryRedirect = Entry.CreateRedirectEntry("", "", true, true);
            entryRegexRedirect = Entry.CreateRegexRedirectEntry("", "", true, true);
        }

        [TestMethod]
        [TestProperty("Number", "17")]
        [TestProperty("Type", "Unit")]
        public void EntryTypeIsCorrectTypeBasedOnDifferentCreateMethod()
        {
            //Arrange & Act
            CreateAllTypesOfEntries();

            //Assert
            entryBlock.EntryType.Should().Be(EntryType.Block);
            entryRegexBlock.EntryType.Should().Be(EntryType.RegexBlock);
            entryRedirect.EntryType.Should().Be(EntryType.Redirect);
            entryRegexRedirect.EntryType.Should().Be(EntryType.RegexRedirect);
        }

        [TestMethod]
        [TestProperty("Number", "37")]
        [TestProperty("Type", "Unit")]
        public void ProfileContainsExactlyAmountOfEntriesThatWereAdded()
        {
            //Arrange
            CreateAllTypesOfEntries();
            Profile testProfile = Profile.CreateProfile("TestProfile", true, true);

            //Act
            testProfile.EntryList.Add(entryBlock);
            testProfile.EntryList.Add(entryRegexBlock);
            testProfile.EntryList.Add(entryRedirect);
            testProfile.EntryList.Add(entryRegexRedirect);

            //Assert
            testProfile.EntryList.Count.Should().Be(4);
        }
    }
}
