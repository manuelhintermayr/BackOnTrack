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
        private RunningApplication _runningApplication;
        public CurrentUserConfiguration UserConfiguration;
        public bool WindowIsShown;
        private string _password;

        public MainView(CurrentUserConfiguration userConfiguration, string password)
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
            App.Current.MainWindow = _runningApplication.UI.MainView; //fix for the dialogWindow
            UserConfiguration = userConfiguration;
            _password = password;
            WindowIsShown = true;
        }

        private void CloseWindowOperations()
        {
            _password = "";
            WindowIsShown = false;
        }

        public void Logout()
        {
            Hide();
            CloseWindowOperations();
            Thread.Sleep(200);

            RunningApplication.Instance().UI.Login.Show();
        }

        private void ModernWindow_Closed(object sender, EventArgs e)
        {
            _runningApplication.MinimizeToTray();
            CloseWindowOperations();
        }

    }
}
