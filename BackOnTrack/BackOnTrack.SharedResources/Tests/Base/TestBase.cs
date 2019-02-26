using BackOnTrack.SharedResources.Infrastructure.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.SharedResources.Tests.Base
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
