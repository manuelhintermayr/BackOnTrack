using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.SharedResources.Tests.Base;
using BackOnTrack.UI.MainView.Pages.Settings;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.Tests
{
    [TestClass]
    public class SettingsWebProxyTests : TestBase
    {
        [DataTestMethod]
        [DataRow("0")]
        [DataRow("1")]
        [DataRow("65535")]
        [TestProperty("Number", "25")]
        [TestProperty("Type", "Unit")]
        public void IsValidPortNumber(string portValue)
        {
            //Arrange & Act
            bool result = SettingsWebProxy.IsValidPortNumber(portValue);

            //Assert
            result.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("-0")]
        [DataRow("-1")]
        [DataRow("65536")]
        [DataRow("65x35")]
        [DataRow("01")]
        [DataRow("")]
        [DataRow("x")]
        [TestProperty("Number", "29")]
        [TestProperty("Type", "Unit")]
        public void IsInvalidPortNumber(string portValue)
        {
            //Arrange & Act
            bool result = SettingsWebProxy.IsValidPortNumber(portValue);

            //Assert
            result.Should().BeFalse();
        }
    }
}
