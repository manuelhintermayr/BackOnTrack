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
        private LoginWindow mainWindow;
        private static MainView instance;
        public MainView(LoginWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
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

            Application.Instance().Login.Show();
        }

        private void ModernWindow_Closed(object sender, EventArgs e)
        {
            mainWindow.Close();
        }

    }
}
