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
    }
}
