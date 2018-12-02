using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using BackOnTrack.Infrastructure.Helpers;

namespace BackOnTrack.UI.MainView.Pages.Tools
{
    /// <summary>
    /// Interaction logic for SystemLevelEditor.xaml
    /// </summary>
    public partial class SystemLevelEditor : UserControl
    {
        public const string HostFileLocation = @"C:\Windows\System32\drivers\etc\hosts";
        private RunningApplication _runningApplication;

        public SystemLevelEditor()
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
        }

        private void LoadSystemSettings_Click(object sender, RoutedEventArgs e)
        {
            if (!HostFileExists())
            {
                NoHostFileGrid.Visibility = Visibility.Visible;
            }
        }

        #region NoHostFile

        public bool HostFileExists()
        {
            return FileModification.FileExists(@"C:\Windows\System32\drivers\etc\hosts");
        }
        private void CreatHostFileButton(object sender, RoutedEventArgs e)
        {
            if (!HostFileExists())
            {
                try
                {
                    FileModification.WriteFile(HostFileLocation, "#Host-file created by BackOnTrack");
                    NoHostFileGrid.Visibility = Visibility.Hidden;
                }
                catch (UnauthorizedAccessException)
                {
                    _runningApplication.UI.MainView.AlertNoAdminRights();
                }
                catch (System.IO.IOException ex)
                {
                    _runningApplication.UI.MainView.AlertErrorWithFile(ex);
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
