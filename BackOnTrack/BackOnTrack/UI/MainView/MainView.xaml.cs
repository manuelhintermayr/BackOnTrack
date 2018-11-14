using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using BackOnTrack.UI.Login;
using FirstFloor.ModernUI.Windows.Controls;

namespace BackOnTrack.UI.MainView
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : ModernWindow
    {
        private static MainView instance;
        public MainView(Services.UserConfiguration.CurrentUserConfiguration userConfiguration)
        {
            InitializeComponent();
            System.Windows.Application.Current.Resources["AccentColor"] = Colors.Teal;
            instance = this;
        }

        public static MainView Instace()
        {
            return instance;
        }

        public void Logout()
        {
            this.Hide();
            Thread.Sleep(200);

            Application.Instance().UI.Login.Show();
        }

        private void ModernWindow_Closed(object sender, EventArgs e)
        {
            Application.Instance().Shutdown();
        }

    }
}
