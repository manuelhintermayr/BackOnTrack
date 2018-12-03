using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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

        public string GetLoggedInPassword()
        {
            return _password;
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


        #region Alerts

        //Alerts
        public void CreateAlertWindow(string title, string content, bool withOkButton = false, RoutedEventHandler eventHandler = null)
        {
            var dlg = new ModernDialog
            {
                Title = title,
                Content = content,
                Owner = this
            };
            if (withOkButton)
            {
                dlg.OkButton.Click += new RoutedEventHandler(eventHandler);
                dlg.Buttons = new Button[] { dlg.OkButton, dlg.CancelButton };
            }
            dlg.ShowDialog();
        }

        public void AlertNoAdminRights()
        {
            CreateAlertWindow("No admin rights", "Please make sure you have admin rights and start the application again.");
        }

        public void AlertErrorWithFile(IOException ioException)
        {
            CreateAlertWindow("Error with file", $"The following error occured with a file:{Environment.NewLine}{Environment.NewLine}{ioException.Message}");
        }

        #endregion
    }
}
