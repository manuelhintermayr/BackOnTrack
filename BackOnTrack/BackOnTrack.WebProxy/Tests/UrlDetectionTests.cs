using BackOnTrack.SharedResources.Tests;
using BackOnTrack.SharedResources.Tests.Base;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.WebProxy.Tests
{
    [TestClass]
    public class UrlDetectionTests : TestBase
    {
        [DataTestMethod]
        [DataRow("\\b(demo)\\b", "https://financeviewer.manuelweb.at/demo/")]
        [DataRow("\\b(demo)\\b", "https://www.google.at/search?client=opera&q=demo&sourceid=opera&ie=UTF-8&oe=UTF-8")]
        [DataRow("http\\b", "http://www.helloworld.com")]
        public void UrlIsMatchWithPattern(string pattern, string address)
        {
            //Arrange & Act
            bool result = LocalWebProxy.AddressIsAMatch(address, pattern);

            //Assert
            result.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("\\b(demo)\\b", "https://financeviewer.manuelweb.at/demostration/")]
        [DataRow("http\\b", "https://www.helloworld.com")]
        public void UrlIsNotAMatchWithPattern(string pattern, string address)
        {
            //Arrange & Act
            bool result = LocalWebProxy.AddressIsAMatch(address, pattern);

            //Assert
            result.Should().BeFalse();
        }
    }
}