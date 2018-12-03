using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        }

        private void SaveProfilesButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //save profiles
            //update webproxy configuration
            UpdateList();
        }

        private void RevertChangesButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //reset configuration
            UpdateList();
        }
    }
}
