using System.Windows;
using System.Windows.Controls;

namespace BackOnTrack.UI.MainView.Pages
{
    /// <summary>
    /// Interaction logic for Logout.xaml
    /// </summary>
    public partial class Logout : UserControl
    {
        private RunningApplication _runningApplication;
        public Logout()
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _runningApplication
                .UI
                .MainView
                .Logout();
        }
    }
}
