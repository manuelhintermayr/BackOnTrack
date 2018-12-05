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
            RemoveNotActiveEntriesFromEntryList(newConfiguration);
            AddMissingEntriesIntoEntryList(newConfiguration);

            HostEntries.Clear();
        }

        private bool HostFileExists()
        {
            return FileModification.FileExists(FileModification.GetHostFileLocation());
        }

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
            var listOfActiveBlockEntries = (
                from profile in newConfiguration.ProfileList from entry in profile.EntryList
                where 
                    entry.EntryType==EntryType.Block && 
                    entry.IsEnabled && 
                    entry.SystemLevelBlockingIsEnabled
                select entry.Url).ToList();

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
            //throw new NotImplementedException();
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
    }
}
