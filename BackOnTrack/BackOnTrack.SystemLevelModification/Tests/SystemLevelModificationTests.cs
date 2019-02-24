﻿using BackOnTrack.SystemLevelModification.Tests.Service;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOnTrack.SystemLevelModification.Tests
{
	[TestClass]
    public class SystemLevelModificationTests
    {
        private SystemLevelModification systemLevelModification;

        public SystemLevelModificationTests()
		{

		}

		[TestMethod]
		public void CheckCorrectWindowsHostsLocationFile()
		{
            //Arrange & Act
            systemLevelModification = new SystemLevelModification(false);

            //Assert
            systemLevelModification.GetHostFileLocation().Should().EndWith(@"Windows\system32\drivers\etc\hosts");
        }

        [TestMethod]
        public void CheckRewriteCorrectWindowsHostsLocationFile()
        {
            //Arrange & Act
            string newHostFileLocation = @"C:\temp\hosts";
            systemLevelModification = new SystemLevelModification(false, newHostFileLocation);

            //Assert
            systemLevelModification.GetHostFileLocation().Should().Be(newHostFileLocation);
        }

        [TestMethod]
        public void CheckHostFileDoesNotExist()
        {
            //todo
            //create temp folder
            //check for not existing host file
            //remove temp folder
        }

        [TestMethod]
        public void CheckHostFileDoesExist()
        {
            //todo
            //create temp folder
            //create empty host file
            //check for existing host file
            //remove temp folder with file
        }

        [TestMethod]
        public void CheckCorrectHostWasCreated()
        {
            using (var tmp = TempFolder.Create())
            {
                string asdf= tmp.Name;
            }

            //todo
            //creaty temp folder
            //create with program host file
            //check if new host file exists
            //remote temp folder with file
            string what = "ok";
        }

        [TestMethod]
        public void CheckHostFileWasReplaced()
        {
            //todo
            //create temp folder
            //create two different host file with different sample content
            //replace with the program one host file with the other 
            //check if host file content has changed
            //remote temp folder with files
        }
    }
}