using System.Windows;
using System.Windows.Controls;

namespace BackOnTrack.UI.MainView.Pages
{
    /// <summary>
    /// Interaction logic for Logout.xaml
    /// </summary>
    public partial class Logout : UserControl
    {
        public Logout()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var mainView = UI.MainView.MainView.Instace();
            mainView.Logout();
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            var mainView = UI.MainView.MainView.Instace();
            mainView.Logout();
        }
    }
}
