using System;
using System.Threading;
using BackOnTrack.Services.UserConfiguration;
using FirstFloor.ModernUI.Windows.Controls;

namespace BackOnTrack.UI.MainView
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : ModernWindow
    {
        private Application _application;
        public CurrentUserConfiguration UserConfiguration;
        private string _password;

        public MainView(CurrentUserConfiguration userConfiguration, string password)
        {
            _application = Application.Instance();
            InitializeComponent();
            UserConfiguration = userConfiguration;
            _password = password;
        }

        public void Logout()
        {
            Hide();
            Thread.Sleep(200);

            Application.Instance().UI.Login.Show();
        }

        private void ModernWindow_Closed(object sender, EventArgs e)
        {
            Application.Instance().Shutdown();
        }

    }
}
