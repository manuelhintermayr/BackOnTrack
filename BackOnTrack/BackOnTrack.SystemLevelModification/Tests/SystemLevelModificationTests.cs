﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BackOnTrack.SharedResources.Infrastructure.Helpers;
using BackOnTrack.SharedResources.Tests;
using BackOnTrack.SharedResources.Tests.Base;

namespace BackOnTrack.SystemLevelModification.Tests
{
	[TestClass]
    public class SystemLevelModificationTests : TestBase
    {
        private SystemLevelModification _systemLevelModification;

		[TestMethod]
        [TestProperty("Number", "44")]
        [TestProperty("Type", "Integration")]
        public void CheckCorrectWindowsHostsLocationFile()
		{
            //Arrange & Act
            _systemLevelModification = new SystemLevelModification(true);

            //Assert
            _systemLevelModification.GetHostFileLocation().Should().EndWith(@"system32\drivers\etc\hosts");
        }

        [TestMethod]
        [TestProperty("Number", "3")]
        [TestProperty("Type", "Unit")]
        public void CheckRewriteCorrectWindowsHostsLocationFile()
        {
            //Arrange & Act
            string newHostFileLocation = @"C:\temp\hosts";
            _systemLevelModification = new SystemLevelModification(true, newHostFileLocation);

            //Assert
            _systemLevelModification.GetHostFileLocation().Should().Be(newHostFileLocation);
        }

        [TestMethod]
        [TestProperty("Number", "10")]
        [TestProperty("Type", "Integration")]
        public void CheckCorrectHostFileWasCreated()
        {
            //Arrange
            string newHostFileLocation = $"{TempFolder.Name}{@"\hosts"}";
            _systemLevelModification = new SystemLevelModification(true, newHostFileLocation);

            //Act
            bool hostFileExistsBefore = _systemLevelModification.HostFileExists();
            _systemLevelModification.CreateNewHostFile();
            bool hostFileExistsAfter = _systemLevelModification.HostFileExists();

            //Assert
            hostFileExistsBefore.Should().BeFalse();
            hostFileExistsAfter.Should().BeTrue();
        }

        [TestMethod]
        [TestProperty("Number", "38")]
        [TestProperty("Type", "Integration")]
        public void CheckHostFileWasReplaced()
        {
            //Arrange
            string newHostFileLocation = $"{TempFolder.Name}{@"\hosts"}";
            _systemLevelModification = new SystemLevelModification(true, newHostFileLocation);
            string newHostFileTwoLocation = $"{TempFolder.Name}{@"\hosts2"}";
            FileModification.WriteFile(newHostFileTwoLocation, "#Test");

            //Act
            _systemLevelModification.CreateNewHostFile();
            string hostFileContentBefore = FileModification.ReadFile(newHostFileLocation);
            _systemLevelModification.ReplaceHostFile(new string[]{ $"-newPath={newHostFileTwoLocation.Replace(" ", "%20")}" });
            string hostFileContentAfter = FileModification.ReadFile(newHostFileLocation);

            //Assert
            hostFileContentAfter.Should().NotBe(hostFileContentBefore);
        }
    }
}
