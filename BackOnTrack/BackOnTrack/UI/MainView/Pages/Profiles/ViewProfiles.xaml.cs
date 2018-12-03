using System;
using System.Collections.Generic;
using System.Globalization;
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
        public CurrentUserConfiguration configuration;

        public ViewProfiles()
        {
            InitializeComponent();
            configuration = new CurrentUserConfiguration() {ProfileList = new List<Profile>()};
            configuration.ProfileList.Add(Profile.CreateProfile("asdf", true, true));
            configuration.ProfileList.Add(Profile.CreateProfile("asdf2", true, true));
            configuration.ProfileList.Add(Profile.CreateProfile("asdf3", true, true));
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            foreach (var profile in configuration.ProfileList)
            {
                ProfileList.Links.Add(new Link(){DisplayName = profile.ProfileName, Source = new Uri(string.Format(CultureInfo.InvariantCulture, "/{0}", profile.ProfileName), UriKind.Relative)});
            }
            //ProfileList.
        }
    }
}
