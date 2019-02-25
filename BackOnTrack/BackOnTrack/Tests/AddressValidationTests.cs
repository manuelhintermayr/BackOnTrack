using System;
using System.Collections.Generic;
using System.Xaml.Schema;
using BackOnTrack.Infrastructure.Exceptions;
using BackOnTrack.SharedResources.Models;
using BackOnTrack.SharedResources.Tests.Base;
using BackOnTrack.UI.MainView.Pages.Profiles;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.Tests
{
    [TestClass]
    public class AddressValidationTests : TestBase
    {
        [DataTestMethod]
        [DataRow("manuelweb")]
        [DataRow("www.manuelweb")]
        [DataRow("manuelweb.at")]
        [DataRow("google.com")]
        [DataRow("1.2.3.4")]
        public void IsCorrectAddress(string address)
        {
            //Arrange & Act
            bool result = AddressValidationRule.IsCorrectAddress(address);
            
            //Assert
            result.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("https://www.manuelweb")]
        [DataRow("+#")]
        [DataRow("go/ogle.com")]
        public void IsInvalidAddress(string address)
        {
            //Arrange & Act
            bool result = AddressValidationRule.IsCorrectAddress(address);

            //Assert
            result.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("\\b(demo)\\b")]
        [DataRow("http\\b")]
        public void IsCorrectRegex(string pattern)
        {
            //Arrange & Act
            bool result = AddressValidationRule.IsCorrectRegex(pattern);

            //Assert
            result.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("\\(demo)\\b")]
        [DataRow("\\b(x]")]
        public void IsInvalidRegex(string pattern)
        {
            //Arrange & Act
            bool result = AddressValidationRule.IsCorrectRegex(pattern);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void FailToAddTheSameAddressTwice()
        {
            //Arrange
            string addressOne = "manuelweb.at";
            string addressTwo = "www.manuelweb.at";
            string addressThree = "www.manuelweb.at";
            string password = "admin";
            RunningApplication runningApplication = new RunningApplication(true, TempFolder.Name);
            runningApplication.Services.UserConfiguration.CreateNewConfiguration(password);
            runningApplication.UI.LoginInMainViewWithoutShowing(password);

            Profile profile = new Profile() { ProfileName = "Manuelweb", EntryList = new List<Entry>() };
            runningApplication.UI.MainView.UserConfiguration.ProfileList.Add(profile);

            SpecificProfileView profileView = new SpecificProfileView("Manuelweb");

            //Act
            Action firstAdd = () => { profileView.AddNewEntry(new Entry(){Url = addressOne}); };
            Action secondAdd = () => { profileView.AddNewEntry(new Entry() { Url = addressTwo }); };
            Action thirdAdd = () => { profileView.AddNewEntry(new Entry() { Url = addressThree }); };

            //Assert
            firstAdd.Should().NotThrow<NewEntryException>();
            secondAdd.Should().NotThrow<NewEntryException>();
            thirdAdd.Should().Throw<NewEntryException>();
        }
    }
}
