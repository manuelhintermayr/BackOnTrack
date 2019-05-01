using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using BackOnTrack.SharedResources.Infrastructure.Helpers;
using BackOnTrack.SharedResources.Tests.Base;
using BackOnTrack.Tests.Base;
using BackOnTrack.UI.MainView.Pages.Tools;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.Tests
{
    [TestClass]
    public class SystemLevelEditorTests : RunningApplicationTestBase
    {
        private SystemLevelEditor _systemLevelEditorPage;

        private void DoSetupAndCreteSystemLevelEditorPage()
        {
            DoSetupWithUnlockingAndBasicHostFile();
            _systemLevelEditorPage = new SystemLevelEditor();
        }

        [TestMethod]
        [TestProperty("Number", "27")]
        [TestProperty("Type", "Integration")]
        public void CheckIfMissingHostFileIsRecognized()
        {
            //Arrange
            DoSetupAndCreteSystemLevelEditorPage();

            //Act
            bool hostFileExists = _systemLevelEditorPage.HostFileExists();

            //Assert
            hostFileExists.Should().BeFalse();
        }

        [TestMethod]
        [TestProperty("Number", "21")]
        [TestProperty("Type", "Integration")]
        public void HostFileWasLoadedCorrect()
        {
            //Arrange
            DoSetupAndCreteSystemLevelEditorPage();
            CreateHostFileWithSampleContent();

            //Act
            _systemLevelEditorPage.FillList();
            var resultList = _systemLevelEditorPage.GetListOfHostEntries();

            //Assert
            resultList.Count.Should().Be(2);
            resultList[0].Content.Should().Be("Test");
            resultList[0].IsEnabled.Should().BeFalse();
            resultList[1].Content.Should().Be("127.0.0.1 facebook.com");
            resultList[1].IsEnabled.Should().BeTrue();
        }

        //Tests HostFileWasSavedCorrect & ContentOfNewCreatedHostFileIsCorrect are already tested in SystemLevelModification
    }
}
