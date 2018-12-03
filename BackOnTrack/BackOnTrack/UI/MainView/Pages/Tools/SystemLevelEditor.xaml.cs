﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using BackOnTrack.Infrastructure.Helpers;

namespace BackOnTrack.UI.MainView.Pages.Tools
{
    /// <summary>
    /// Interaction logic for SystemLevelEditor.xaml
    /// </summary>
    public partial class SystemLevelEditor : UserControl
    {
        List<HostEntry> HostEntries = new List<HostEntry>();
        int CurrentLineNumber = 0;

        public string GetHostFileLocation()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"system32\drivers\etc\hosts");
        }

        private RunningApplication _runningApplication;

        public SystemLevelEditor()
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
        }

        private void LoadSystemSettings_Click(object sender, RoutedEventArgs e)
        {
            LoadSystemSettings.Content = "Reload system settings";

            if (HostEntriesList.Visibility == Visibility.Visible)
            {
                string alertTitle = "Reload system settings";
                string alertContent =
                    "Are you sure you want to reload the system settings? This will reset all not saved changes.";
                _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent, true, new RoutedEventHandler(LoadSystemSettingsAction));
            }
            else
            {
                LoadSystemSettingsAction(sender, e);
            }
        }


        private void LoadSystemSettingsAction(object sender, RoutedEventArgs e)
        {
            bool hostEntryWhereBeforeVisible = HostEntriesList.Visibility == Visibility.Visible;

            if (!HostFileExists())
            {
                NoHostFileGrid.Visibility = Visibility.Visible;
                SaveSystemSettings.Visibility = Visibility.Hidden;
                HostEntryEnDisAble.Visibility = Visibility.Hidden;
                HostEntriesList.Visibility = Visibility.Hidden;
            }
            else
            {
                HostEntriesList.Visibility = Visibility.Hidden; //fix to really reload all entries
                FillList();
                NoHostFileGrid.Visibility = Visibility.Hidden;
                SaveSystemSettings.Visibility = Visibility.Visible;
                HostEntryEnDisAble.Visibility = Visibility.Visible;
                HostEntriesList.Visibility = Visibility.Visible;
            }

            if (hostEntryWhereBeforeVisible)
            {
                string alertTitle = "System settings reloaded";
                string alertContent = "The system settings where successfully reloaded!";

                _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent);
            }
        }

        #region NoHostFile

        public bool HostFileExists()
        {
            return FileModification.FileExists(GetHostFileLocation());
        }
        private void CreatHostFileButton(object sender, RoutedEventArgs e)
        {
            if (!HostFileExists())
            {
                int result = _runningApplication.Services.SystemLevelConfiguration.CreateNewHostFile();
                if(result==0)
                {
                    _runningApplication.UI.MainView.CreateAlertWindow("SystemFile successfully created.", "The SystemFile was successfully created!");
                    NoHostFileGrid.Visibility = Visibility.Hidden;
                }
                else if(result==1)
                {
                    _runningApplication.UI.MainView.CreateAlertWindow("Creating system file failed.", "Creating the new system file failed. Information was given in the previous window.");
                }
                else
                {
                    _runningApplication.UI.MainView.CreateAlertWindow("Closing error.", "The tool for SystemFileModification was not closed in a correct way.");
                }

            }
            else
            {
                NoHostFileGrid.Visibility = Visibility.Hidden;
            }
        }

        #endregion

        #region LoadHostFile
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            EntryList.ItemsSource = HostEntries;
            foreach(var colum in EntryList.Columns)
            {
                if (colum.Header.ToString().Contains("LineNumber"))
                {
                    colum.Header = "#";
                }
                if (colum.Header.ToString().Contains("IsEnabled"))
                {
                    colum.Header = "Is enabled";
                }
            }
        }

        private void EntryList_AutoGeneratedColumns(object sender, EventArgs e)
        {
            EntryList.Columns.FirstOrDefault(x => x.Header.ToString() == "LineNumber").DisplayIndex = 0;
        }

        private void FillList()
        {
            HostEntries.Clear();
            CurrentLineNumber = 0;
            AddAllLinesFromHostFileIntoEntryList();
            TransformAllLinesInEntryList();
            EntryList.Items.Refresh();
        }

        private void TransformAllLinesInEntryList()
        {
            for (int i = 0; i < HostEntries.Count;i++)
            {
                var currentEntry = HostEntries[i];
                if (currentEntry.Content.StartsWith("#"))
                {
                    currentEntry.Content = currentEntry.Content.Substring(1);
                }
                else
                {
                    currentEntry.IsEnabled = true;
                }
            }
        }

        private void AddAllLinesFromHostFileIntoEntryList()
        {
            string fileContent = FileModification.ReadFile(GetHostFileLocation());

            using (StringReader reader = new StringReader(fileContent))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    HostEntry newEntry = new HostEntry() { Content = line, LineNumber = (CurrentLineNumber + 1), IsEnabled = false};
                    HostEntries.Add(newEntry);
                    CurrentLineNumber++;
                }
            }

        }

        #endregion

        #region Change Entries

        private void EntryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEnDisAbleDeleteButtonsVisibility();
        }

        private void UpdateEnDisAbleDeleteButtonsVisibility()
        {
            if (EntryList.SelectedItems.Count > 1 || EntryList.SelectedItem != null)
            {
                EnableEntries.IsEnabled = true;
                DisableEntries.IsEnabled = true;
                DeleteEntries.IsEnabled = true;
            }
            else
            {
                EnableEntries.IsEnabled = false;
                DisableEntries.IsEnabled = false;
                DeleteEntries.IsEnabled = false;
            }
        }

        private void EnableEntries_Click(object sender, RoutedEventArgs e)
        {
            EnDisAbleSelectedEntries(true);
        }

        private void DisableEntries_Click(object sender, RoutedEventArgs e)
        {
            EnDisAbleSelectedEntries(false);
        }

        private void EnDisAbleSelectedEntries(bool enable)
        {
            if (EntryList.SelectedItems.Count > 1)
            {
                System.Collections.IList items = (System.Collections.IList)EntryList.SelectedItems;
                var selectedItems = items.Cast<HostEntry>();
                foreach (var entry in selectedItems)
                {
                    entry.IsEnabled = enable;
                }
            }
            else
            {
                if (EntryList.SelectedItem != null)
                {
                    HostEntry entry = (HostEntry)EntryList.SelectedValue;
                    entry.IsEnabled = enable;
                }
            }
            EntryList.Items.Refresh();
        }

        private void DeleteEntries_Click(object sender, RoutedEventArgs e)
        {
            string alertTitle = "Delete entrie(s)";
            string alertContent =
                "Are you sure you want to delete the selected entry / entries?";
            _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent, true, new RoutedEventHandler(DeleteEntriesAction));
        }

        private void DeleteEntriesAction(object sender, RoutedEventArgs e)
        {
            if (EntryList.SelectedItems.Count > 1)
            {
                System.Collections.IList items = (System.Collections.IList)EntryList.SelectedItems;
                var selectedItems = items.Cast<HostEntry>();

                foreach (var entry in selectedItems)
                {
                    HostEntries.Remove(entry);
                }
            }
            else
            {
                if (EntryList.SelectedItem != null)
                {
                    HostEntry entry = (HostEntry)EntryList.SelectedValue;
                    HostEntries.Remove(entry);
                }
            }

            EntryList.Items.Refresh();
            UpdateEnDisAbleDeleteButtonsVisibility();
        }

        #endregion

        #region Add Entry

        private void AddNewEntry_Click(object sender, RoutedEventArgs e)
        {
            if (DomainToBlockTextbox.Text == "" || DomainToBlockTextbox.Text.Contains(" "))
            {
                _runningApplication.UI.MainView.CreateAlertWindow("Correct domain value", "Please enter a correct value for a domain or a IP-address to block.");
            }
            else
            {
                HostEntries.Add(new HostEntry() { Content = $"127.0.0.1   {DomainToBlockTextbox.Text}", IsEnabled = true, LineNumber = (CurrentLineNumber + 1) });
                DomainToBlockTextbox.Text = "";
                CurrentLineNumber++;
                EntryList.Items.Refresh();
            }
        }

        #endregion

        #region Save Entries

        private void SaveSystemSettings_Click(object sender, RoutedEventArgs e)
        {
            string alertTitle = "Save current system settings";
            string alertContent =
                "Are you sure you want to save the current configuration in the editor to the system file?";
            _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent, true, new RoutedEventHandler(SaveCurrentSystemSettings));
        }

        private void SaveCurrentSystemSettings(object sender, RoutedEventArgs e)
        {
            string hostFileContent = BuildHostFileContent();

            int result = _runningApplication.Services.SystemLevelConfiguration.UpdateHostFile(hostFileContent);
            if (result == 0)
            {
                _runningApplication.UI.MainView.CreateAlertWindow("SystemFile was successfully updated.", "The SystemFile was successfully updated!");
                NoHostFileGrid.Visibility = Visibility.Hidden;
            }
            else if (result == 1)
            {
                _runningApplication.UI.MainView.CreateAlertWindow("Updating system file failed.", "Updating the system file failed. Information was given in the previous window.");
            }
            else
            {
                _runningApplication.UI.MainView.CreateAlertWindow("Closing error.", "The tool for SystemFileModification was not closed in a correct way.");
            }
        }

        private string BuildHostFileContent()
        {
            string hostFileContent = "";
            for (int i = 0; i < HostEntries.Count; i++)
            {
                var hostEntry = HostEntries[i];
                hostFileContent = $"{hostFileContent}{(hostEntry.IsEnabled ? "" : "#")}{hostEntry.Content}";
                if ((i + 1) != HostEntries.Count) //checking for last entry 
                {
                    hostFileContent = $"{hostFileContent}{Environment.NewLine}";
                }
            }

            return hostFileContent;
        }

        #endregion

    }
}
