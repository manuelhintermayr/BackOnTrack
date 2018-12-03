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
    /// Interaction logic for AddProfile.xaml
    /// </summary>
    public partial class AddProfile : UserControl
    {
        private RunningApplication _runningApplication;

        public AddProfile()
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
        }

        private void AddProfileButton_Click(object sender, RoutedEventArgs e)
        {
            string newProfileName = NewProfileName.Text;
            
            if (newProfileName == "" || newProfileName == " " || newProfileName.Length > 48 || CheckIfProfileNameIsAlreadyUsed(newProfileName))
            {
                string alertTitle = "Error with new profile name.";
                string alertContent = "Could not create a new profile. The new profile name may be invalid, too long or already used.";
                _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent);
            }
            else
            {


                Profile newProfile = Profile.CreateProfile(newProfileName, EnableBlockingOnProxyLevel.IsChecked.Value,
                    EnableBlockingOnSystemLevel.IsChecked.Value);
                _runningApplication.UI.MainView.UserConfiguration.ProfileList.Add(newProfile);

                NewProfileName.Text = "";
                EnableBlockingOnProxyLevel.IsChecked = true;
                EnableBlockingOnProxyLevel.UpdateLayout();
                EnableBlockingOnSystemLevel.IsChecked = true;

                ((FirstFloor.ModernUI.Windows.Controls.ModernFrame)(this.Parent)).Source = new Uri("/UI/MainView/Pages/Profiles/ViewProfiles.xaml", UriKind.Relative);
            }
        }

        private bool CheckIfProfileNameIsAlreadyUsed(string profileName)
        {
            return _runningApplication.UI.MainView.UserConfiguration.ProfileList.Select(x => x.ProfileName).ToList()
                .Contains(profileName);
        }
    }
}
