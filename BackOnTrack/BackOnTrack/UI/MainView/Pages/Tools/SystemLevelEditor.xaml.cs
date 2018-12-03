using System;
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
            if (!HostFileExists())
            {
                NoHostFileGrid.Visibility = Visibility.Visible;
                SaveSystemSettings.Visibility = Visibility.Hidden;
                HostEntryEnDisAble.Visibility = Visibility.Hidden;
                HostEntriesList.Visibility = Visibility.Hidden;
            }
            else
            {
                NoHostFileGrid.Visibility = Visibility.Hidden;
                SaveSystemSettings.Visibility = Visibility.Visible;
                HostEntryEnDisAble.Visibility = Visibility.Visible;
                HostEntriesList.Visibility = Visibility.Visible;

                FillList();
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
            
            //HostEntries.Sort(new Comparison<>());
            var x = EntryList.Columns[0];
        }
        //class ColumComparer : IComparer
        //{
        //    public int Compare(Object q, Object r)
        //    {
        //        string first = q
        //        Point a = (p)q;
        //        Point b = (p)r;
        //        if ((a.x == b.x) && (a.y == b.y))
        //            return 0;
        //        if ((a.x < b.x) || ((a.x == b.x) && (a.y < b.y)))
        //            return -1;

        //        return 1;
        //    }
        //}


        private void FillList()
        {
            HostEntries.Clear();
            CurrentLineNumber = 0;
            AddAllLinesFromHostFileIntoEntryList();
            TransformAllLinesInEntryList();
            this.UpdateLayout();
            this.UpdateDefaultStyle();
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

        private void EnDisAbleEntries_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteEntries_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Add Entry

        private void AddNewEntry_Click(object sender, RoutedEventArgs e)
        {
            HostEntries.Add(new HostEntry(){Content = $"127.0.0.1   {DomainToBlockTextbox.Text}", IsEnabled = true, LineNumber = (CurrentLineNumber+1)});
            DomainToBlockTextbox.Text = "";
            CurrentLineNumber++;
        }

        #endregion

        #region Save Entries

        private void SaveSystemSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
    }
}
