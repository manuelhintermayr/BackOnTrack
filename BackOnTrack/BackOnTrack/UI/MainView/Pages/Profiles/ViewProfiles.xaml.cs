using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BackOnTrack.Services.UserConfiguration;
using FirstFloor.ModernUI.Presentation;

namespace BackOnTrack.UI.MainView.Pages.Profiles
{
    /// <summary>
    /// Interaction logic for ViewProfiles.xaml
    /// </summary>
    public partial class ViewProfiles : UserControl
    {
        public RunningApplication _runningApplication;

        public ViewProfiles()
        {
            InitializeComponent();
            _runningApplication = RunningApplication.Instance();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            NoProfilesYetGrid.Visibility = Visibility.Hidden;

            //remove old entries
            var profileNameList = _runningApplication.UI.MainView.UserConfiguration.ProfileList
                .Select(x => x.ProfileName).ToList();
            for (int i = ProfileList.Links.Count - 1; i >= 0; i--)
            {
                if (!profileNameList.Contains(ProfileList.Links[i].DisplayName))
                {
                    ProfileList.Links.RemoveAt(i);
                }

            }

            //add new entries
            var uiProfileNameList = ProfileList.Links.Select(x => x.DisplayName).ToList();
            foreach (var profile in _runningApplication.UI.MainView.UserConfiguration.ProfileList)
            {
                if (!uiProfileNameList.Contains(profile.ProfileName))
                {
                    ProfileList.Links.Add(new Link() { DisplayName = profile.ProfileName, Source = new Uri(string.Format(CultureInfo.InvariantCulture, "/{0}", profile.ProfileName), UriKind.Relative) });
                }
            }

            if (ProfileList.Links.Count == 0)
            {
                NoProfilesYetGrid.Visibility = Visibility.Visible;
            }
        }

        private void SaveProfilesButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //save profiles
            //update webproxy configuration
            UpdateList();
        }

        private void RevertChangesButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string alertTitle = "Revert changes";
            string alertContent =
                $"This will revert all changes you made to your user profile since the last save and will reset the {Environment.NewLine}configuration to the current loaded. {Environment.NewLine}{Environment.NewLine}Are you sure you want to do this?";
            var alertOkEvent = new RoutedEventHandler(RevertConfigurationOkClick);
        
            _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent, true, alertOkEvent);        
        }

        private void RevertConfigurationOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var originalConfiguration =
                    _runningApplication.Services.UserConfiguration.OpenConfiguration(_runningApplication.UI.MainView
                        .GetLoggedInPassword());

                _runningApplication.UI.MainView.SetCurrentUserConfiguration(originalConfiguration);
                UpdateList();
            }
            catch (Exception ex)
            {
                string alertTitle = "Error reloading configuration.";
                string alertContent =
                    $"Could not reload the configuration. Following error occured:{Environment.NewLine}{ex}";

                _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent);
            }


        }
    }
}
