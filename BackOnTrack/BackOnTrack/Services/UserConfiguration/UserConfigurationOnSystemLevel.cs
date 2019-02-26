using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BackOnTrack.Infrastructure.Exceptions;
using BackOnTrack.SharedResources.Infrastructure.Helpers;
using BackOnTrack.SharedResources.Models;

namespace BackOnTrack.Services.UserConfiguration
{
    public class UserConfigurationOnSystemLevel
    {
        private List<HostEntry> _hostEntries = new List<HostEntry>();
        private RunningApplication _runningApplication;

        public UserConfigurationOnSystemLevel()
        {
            _runningApplication = RunningApplication.Instance();
        }

        public List<HostEntry> GetHostEntries()
        {
            return _hostEntries;
        }

        public void ApplyingConfiguration(CurrentUserConfiguration newConfiguration)
        {
            if (!HostFileExists())
            {
                CreateHostFile();
            }

            AddAllLinesFromHostFileIntoEntryList();
            AddMissingEntriesIntoEntryList(newConfiguration);
            RemoveNotActiveEntriesFromEntryList(newConfiguration);

            SaveHostFile();
            _hostEntries.Clear();
        }

        #region HostFile manipulation

        private bool HostFileExists()
        {
            return FileModification.FileExists(FileModification.GetHostFileLocation());
        }

        private void CreateHostFile()
        {
            int result = _runningApplication.Services.SystemLevelConfiguration.CreateNewHostFile();
            if (result == 1)
            {
                throw new SystemLevelException("Creating the new system file failed. Information was given in the previous window.");
            }
            else if (result != 0)
            {
                throw new SystemLevelException("The tool for SystemFileModification was not closed in a correct way.");
            }
        }

        private void SaveHostFile()
        {
            string hostFileContent = BuildHostFileContent();
            string oldHostFileContent = GetHostFileContent();
            if (hostFileContent != oldHostFileContent)
            {
                int result = _runningApplication.Services.SystemLevelConfiguration.UpdateHostFile(hostFileContent);
                if (result == 1)
                {
                    throw new SystemLevelException("Updating the system file failed. Information was given in the previous window.");
                }
                else if (result != 0)
                {
                    throw new SystemLevelException("The tool for SystemFileModification was not closed in a correct way.");
                }
            }
        }

        #endregion

        private string GetHostFileContent()
        {
            return FileModification.ReadFile(FileModification.GetHostFileLocation());
        }
        public void AddAllLinesFromHostFileIntoEntryList()
        {
            string fileContent = GetHostFileContent();

            using (StringReader reader = new StringReader(fileContent))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    HostEntry newEntry = new HostEntry() { Content = line };
                    _hostEntries.Add(newEntry);
                }
            }

        }

        public void AddMissingEntriesIntoEntryList(CurrentUserConfiguration newConfiguration)
        {
            List<string> listOfActiveBlockEntries = GetListOfActiveBlockEntries(newConfiguration);

            List<string> stringListOfEntries = _hostEntries.Select(x => x.Content).ToList();

            if (listOfActiveBlockEntries.Count != 0)
            {
                foreach (string blockedAddress in listOfActiveBlockEntries)
                {
                    string entryLine = $"127.0.0.1  {blockedAddress} #BackOnTrackEntry";
                    if (!stringListOfEntries.Contains(entryLine))
                    {
                        _hostEntries.Add(new HostEntry() { Content = entryLine });
                    }
                }
            }
        }

        public void RemoveNotActiveEntriesFromEntryList(CurrentUserConfiguration newConfiguration)
        {
            List<string> listOfActiveBlockEntries = GetListOfActiveBlockEntries(newConfiguration).Select(x=> $"127.0.0.1  {x} #BackOnTrackEntry").ToList();

            for (int i = _hostEntries.Count - 1; i >= 0; i--)
            {
                var currentHostEntry = _hostEntries[i];
                if (currentHostEntry.Content.Contains("#BackOnTrackEntry"))
                {
                    //this is a #BackOnTrackEntry
                    if (!listOfActiveBlockEntries.Contains(currentHostEntry.Content))
                    {
                        //old entry
                        _hostEntries.RemoveAt(i);
                    }
                }
            }
        }

        private string BuildHostFileContent()
        {
            string hostFileContent = "";
            for (int i = 0; i < _hostEntries.Count; i++)
            {
                var hostEntry = _hostEntries[i];
                hostFileContent = $"{hostFileContent}{hostEntry.Content}";
                if ((i + 1) != _hostEntries.Count) //checking for last entry 
                {
                    hostFileContent = $"{hostFileContent}{Environment.NewLine}";
                }
            }

            return hostFileContent;
        }

        public static List<string> GetListOfActiveBlockEntries(CurrentUserConfiguration usedConfiguration)
        {
            return (
                from profile in usedConfiguration.ProfileList
                from entry in profile.EntryList
                where
                    profile.ProfileIsEnabled &&
                    entry.EntryType == EntryType.Block &&
                    entry.IsEnabled &&
                    entry.SystemLevelBlockingIsEnabled
                select entry.Url
                ).ToList();
        }
    }
}
