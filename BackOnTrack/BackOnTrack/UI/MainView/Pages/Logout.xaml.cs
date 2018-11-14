using System.Windows;
using System.Windows.Controls;

namespace BackOnTrack.UI.MainView.Pages
{
    /// <summary>
    /// Interaction logic for Logout.xaml
    /// </summary>
    public partial class Logout : UserControl
    {
        private Application _application;
        public Logout()
        {
            _application = Application.Instance();
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _application
                .UI
                .MainView
                .Logout();
        }
    }
}
