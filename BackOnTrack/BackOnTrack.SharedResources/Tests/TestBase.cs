using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.SharedResources.Infrastructure.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.SharedResources.Tests
{
    [TestClass]
    public class TestBase
    {
        public TestContext TestContext { get; set; }
        public TempFolder TempFolder;

        [TestInitialize]
        public void TestIntialize()
        {
            string testMethodName = TestContext.TestName;
            TempFolder = new TempFolder(testMethodName);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            TempFolder.Dispose();
        }
    }
}
