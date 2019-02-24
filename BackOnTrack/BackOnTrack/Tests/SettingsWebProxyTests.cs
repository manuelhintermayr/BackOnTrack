using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.SharedResources.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.Tests
{
    [TestClass]
    public class SettingsWebProxyTests : TestBase
    {
        [TestMethod]
        public void IsValidPortNumber()
        {
            //extract and check regex (from ProxyPortAddress_TextChanged)
        }

        [TestMethod]
        public void IsInvalidPortNumber()
        {
            //
        }
    }
}
