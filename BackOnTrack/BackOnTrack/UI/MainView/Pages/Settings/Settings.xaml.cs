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
            _runningApplication.Services.ProgramConfiguration.SetCurrentConfigurationFromConfig();

            string alertTitle = "Configuration reloaded";
            string alertContent = "The configuration was successfully reloaded!";

            CreateAlertWindow(alertTitle, alertContent);
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
            throw new NotImplementedException();

            string alertTitle = "Configuration saved";
            string alertContent = "The configuration was successfully saved!";

            CreateAlertWindow(alertTitle, alertContent);
        }

        //Revert Configuration
        private void RevertChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string alertTitle = "Revert changes";
            string alertContent =
                $"This will revert all changes you made during this session and will reset and load the configuration to {Environment.NewLine}the one you had at the beginning of this session. {Environment.NewLine}{Environment.NewLine}Are you sure you want to do this?";
            var alertOkEvent = new RoutedEventHandler(RevertConfigurationOkClick);

            CreateAlertWindow(alertTitle, alertContent, true, alertOkEvent);
        }

        private void RevertConfigurationOkClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();

            string alertTitle = "Configuration changes reverted";
            string alertContent = "The configuration changes where successfully reverted!";

            CreateAlertWindow(alertTitle, alertContent);
        }


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
    }
}
