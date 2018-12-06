using System;
using System.Windows;
using System.Windows.Controls;

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

        //Reset Configuration
        private void ResetConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            string alertTitle = "Reset Configuration";
            string alertContent =
                $"This will reset, save and load your current program configuration to a default one. {Environment.NewLine}{Environment.NewLine}Are you sure you want to do this?";
            var alertOkEvent = new RoutedEventHandler(ResetConfigurationOkClick);

            _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent, true, alertOkEvent);
        }
        private void ResetConfigurationOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _runningApplication.Services.ProgramConfiguration.SetCurrentConfigurationToDefault();

                string alertTitle = "Configuration reset";
                string alertContent = "The configuration was successfully reset!";

                _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent);
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

        //Save Configuration
        private void SaveConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            string alertTitle = "Save Configuration";
            string alertContent =
                $"This will save and load your current program configuration. {Environment.NewLine}{Environment.NewLine}Are you sure you want to do this?";
            var alertOkEvent = new RoutedEventHandler(SaveConfigurationOkClick);

            _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent, true, alertOkEvent);
        }
        private void SaveConfigurationOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _runningApplication.Services.ProgramConfiguration.SaveCurrentConfiguration();

                string alertTitle = "Configuration saved";
                string alertContent = "The configuration was successfully saved!";

                _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent);
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

        //Revert Configuration
        private void RevertChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string alertTitle = "Revert changes";
            string alertContent =
                $"This will revert all changes you made since the last save and will reset the configuration to {Environment.NewLine}the current loaded. {Environment.NewLine}{Environment.NewLine}Are you sure you want to do this?";
            var alertOkEvent = new RoutedEventHandler(RevertConfigurationOkClick);

            _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent, true, alertOkEvent);
        }

        private void RevertConfigurationOkClick(object sender, RoutedEventArgs e)
        {
            _runningApplication.Services.ProgramConfiguration.RevertChangesFromCurrentConfig();

            string alertTitle = "Configuration changes reverted";
            string alertContent = "The configuration changes where successfully reverted!";

            _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent);
        }

    }
}
