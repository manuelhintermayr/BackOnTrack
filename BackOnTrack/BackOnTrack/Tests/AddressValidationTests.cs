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
            //
        }
    }
}
