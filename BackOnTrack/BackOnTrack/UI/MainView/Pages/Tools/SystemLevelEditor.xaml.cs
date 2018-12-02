using System;
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
            }
            else
            {
                NoHostFileGrid.Visibility = Visibility.Hidden;
                SaveSystemSettings.Visibility = Visibility.Visible;
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

        private void SaveSystemSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
