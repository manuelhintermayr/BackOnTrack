using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.Infrastructure.Exceptions;
using BackOnTrack.Infrastructure.Helpers;

namespace BackOnTrack.Services.UserConfiguration
{
    public class UserConfigurationOnSystemLevel
    {
        private List<HostEntry> HostEntries = new List<HostEntry>();
        private RunningApplication _runningApplication;

        public UserConfigurationOnSystemLevel()
        {
            _runningApplication = RunningApplication.Instance();
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
            HostEntries.Clear();
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
            int result = _runningApplication.Services.SystemLevelConfiguration.UpdateHostFile(hostFileContent);
            if (result == 1)
            {
                throw new SystemLevelException("Updating the system file failed. Information was given in the previous window.");
            }
            else if(result != 0)
            {
                throw new SystemLevelException("The tool for SystemFileModification was not closed in a correct way.");
            }
        }

        #endregion


        private void AddAllLinesFromHostFileIntoEntryList()
        {
            string fileContent = FileModification.ReadFile(FileModification.GetHostFileLocation());

            using (StringReader reader = new StringReader(fileContent))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    HostEntry newEntry = new HostEntry() { Content = line };
                    HostEntries.Add(newEntry);
                }
            }

        }

        private void AddMissingEntriesIntoEntryList(CurrentUserConfiguration newConfiguration)
        {
            List<string> listOfActiveBlockEntries = GetListOfActiveBlockEntries(newConfiguration);

            List<string> stringListOfEntries = HostEntries.Select(x => x.Content).ToList();

            if (listOfActiveBlockEntries.Count != 0)
            {
                foreach (string blockedAddress in listOfActiveBlockEntries)
                {
                    string entryLine = $"127.0.0.1  {blockedAddress} #BackOnTrackEntry";
                    if (!stringListOfEntries.Contains(entryLine))
                    {
                        HostEntries.Add(new HostEntry() { Content = entryLine });
                    }
                }
            }
        }

        private void RemoveNotActiveEntriesFromEntryList(CurrentUserConfiguration newConfiguration)
        {
            List<string> listOfActiveBlockEntries = GetListOfActiveBlockEntries(newConfiguration).Select(x=> $"127.0.0.1  {x} #BackOnTrackEntry").ToList();

            for (int i = HostEntries.Count - 1; i >= 0; i--)
            {
                var currentHostEntry = HostEntries[i];
                if (currentHostEntry.Content.Contains("#BackOnTrackEntry"))
                {
                    //this is a #BackOnTrackEntry
                    if (!listOfActiveBlockEntries.Contains(currentHostEntry.Content))
                    {
                        //old entry
                        HostEntries.RemoveAt(i);
                    }
                }
            }
        }

        private string BuildHostFileContent()
        {
            string hostFileContent = "";
            for (int i = 0; i < HostEntries.Count; i++)
            {
                var hostEntry = HostEntries[i];
                hostFileContent = $"{hostFileContent}{hostEntry.Content}";
                if ((i + 1) != HostEntries.Count) //checking for last entry 
                {
                    hostFileContent = $"{hostFileContent}{Environment.NewLine}";
                }
            }

            return hostFileContent;
        }

        private List<string> GetListOfActiveBlockEntries(CurrentUserConfiguration usedConfiguration)
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
