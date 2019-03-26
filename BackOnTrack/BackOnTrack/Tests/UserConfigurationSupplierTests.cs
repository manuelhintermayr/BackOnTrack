using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.Services.UserConfiguration;
using BackOnTrack.SharedResources.Models;
using BackOnTrack.SharedResources.Tests.Base;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.Tests
{
    [TestClass]
    public class UserConfigurationSupplierTests : TestBase
    {
        [TestMethod]
        [TestProperty("Number", "13")]
        [TestProperty("Type", "Unit")]
        public void ConfigurationWasSuccessfullyLoadedFromEnryptedText()
        {
            //Arrange
            string encryptedUserConfiguration = "QLiHt6rADqprmc0LqINtIxV2q+jApusXxgXL5KoHhLefOl+kKRPcFQQD0E9lIPo0L0UMjhSHdLvLOG0/GFUoQjYlB8RednefsikQDLXQMLFXSoyHMjWsqw/wODSnn4Ke";
            string passwordToDecrypt = "admin";
            CurrentUserConfiguration userConfigurationToCheck = new CurrentUserConfiguration() { ProfileList = new List<Profile>() };

            //Act
            var result = UserConfigurationSupplier.DecryptConfiguration(encryptedUserConfiguration, passwordToDecrypt);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(userConfigurationToCheck);
        }
    }
}
