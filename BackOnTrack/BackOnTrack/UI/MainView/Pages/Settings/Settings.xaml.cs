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
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Controls;

namespace BackOnTrack.UI.MainView.Pages.Settings
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        private RunningApplication _runningApplication;

        public Settings()
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
        }

        //Reload Configuration
        private void ReloadConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            string alertTitle = "Reload Configuration";
            string alertContent =
                $"This will reset and load your current program configuration to the current saved configuration. {Environment.NewLine}Are you sure you want to do this?";
            var alertOkEvent = new RoutedEventHandler(ResetConfigurationOkClick);

            CreateAlertWindow(alertTitle, alertContent, true, alertOkEvent);
        }
        private void ResetConfigurationOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _runningApplication.Services.ProgramConfiguration.SetCurrentConfigurationFromConfig();

                string alertTitle = "Configuration reloaded";
                string alertContent = "The configuration was successfully reloaded!";

                CreateAlertWindow(alertTitle, alertContent);
            }
            catch (UnauthorizedAccessException)
            {
                AlertNoAdminRights();
            }
            catch (System.IO.IOException ex)
            {
                AlertErrorWithFile(ex);
            }
        }

        //Save Configuration
        private void SaveConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            string alertTitle = "Save Configuration";
            string alertContent =
                $"This will save and load your current program configuration. {Environment.NewLine}Are you sure you want to do this?";
            var alertOkEvent = new RoutedEventHandler(SaveConfigurationOkClick);

            CreateAlertWindow(alertTitle, alertContent, true, alertOkEvent);
        }
        private void SaveConfigurationOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _runningApplication.Services.ProgramConfiguration.SaveCurrentConfiguration();

                string alertTitle = "Configuration saved";
                string alertContent = "The configuration was successfully saved!";

                CreateAlertWindow(alertTitle, alertContent);
            }
            catch (UnauthorizedAccessException)
            {
                AlertNoAdminRights();
            }
            catch (System.IO.IOException ex)
            {
                AlertErrorWithFile(ex);
            }
        }

        //Revert Configuration
        private void RevertChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string alertTitle = "Revert changes";
            string alertContent =
                $"This will revert all changes you made since the last save and will reset and load the configuration to {Environment.NewLine}the current loaded. {Environment.NewLine}{Environment.NewLine}Are you sure you want to do this?";
            var alertOkEvent = new RoutedEventHandler(RevertConfigurationOkClick);

            CreateAlertWindow(alertTitle, alertContent, true, alertOkEvent);
        }

        private void RevertConfigurationOkClick(object sender, RoutedEventArgs e)
        {
            _runningApplication.Services.ProgramConfiguration.RevertChangesFromCurrentConfig();

            string alertTitle = "Configuration changes reverted";
            string alertContent = "The configuration changes where successfully reverted!";

            CreateAlertWindow(alertTitle, alertContent);
        }

        //Alerts
        private void CreateAlertWindow(string title, string content, bool withOkButton = false, RoutedEventHandler eventHandler = null)
        {
            var dlg = new ModernDialog
            {
                Title = title,
                Content = content,
                Owner = _runningApplication.UI.MainView
            };
            if (withOkButton)
            {
                dlg.OkButton.Click += new RoutedEventHandler(eventHandler);
                dlg.Buttons = new Button[] { dlg.OkButton, dlg.CancelButton };
            }
            dlg.ShowDialog();
        }

        private void AlertNoAdminRights()
        {
            CreateAlertWindow("No admin rights", "Please make sure you have admin rights and start the application again.");
        }

        private void AlertErrorWithFile(IOException ioException)
        {
            CreateAlertWindow("Error with file", $"The following error occured with a file:{Environment.NewLine}{Environment.NewLine}{ioException.Message}");
        }
    }
}
