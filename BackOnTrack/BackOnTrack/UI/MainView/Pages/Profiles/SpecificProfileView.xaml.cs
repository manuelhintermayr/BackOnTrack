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
using BackOnTrack.Services.UserConfiguration;

namespace BackOnTrack.UI.MainView.Pages.Profiles
{
    /// <summary>
    /// Interaction logic for SpecificProfileView.xaml
    /// </summary>
    public partial class SpecificProfileView : UserControl
    {
        private RunningApplication _runningApplication;
        public Profile CurrentProfile { get; set; }

        public SpecificProfileView(string profileName)
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
            this.DataContext = this;
            Setup(profileName);
        }

        private void Setup(string profileName)
        {
            foreach (var profile in _runningApplication.UI.MainView.UserConfiguration.ProfileList)
            {
                if (profile.ProfileName == profileName)
                {
                    CurrentProfile = profile;
                    break;
                }
            }

            ProfileName.Text = profileName;
        }

        private void DeleteCurrentProfile_Click(object sender, RoutedEventArgs e)
        {
            string alertTitle = "Remove profile";
            string alertContent =
                "Are you sure you want to delete this profile? Changes become active after saving.";
            var alertOkEvent = new RoutedEventHandler(DeleteProfileOkClick);

            _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent, true, alertOkEvent);
        }

        private void DeleteProfileOkClick(object sender, RoutedEventArgs e)
        {
            if (!_runningApplication.UI.MainView.UserConfiguration.ProfileList.Remove(CurrentProfile))
            {
                _runningApplication.UI.MainView.CreateAlertWindow("Error removing profile", "Could not remove the profile.");
            }
            else
            {
                _runningApplication.UI.MainView.ContentSource = new Uri("/UI/MainView/Pages/Profiles/ViewProfiles.xaml", UriKind.Relative);
            }

        }
    }
}
