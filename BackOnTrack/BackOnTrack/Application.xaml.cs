using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using BackOnTrack.Infrastructure.Helpers;
using BackOnTrack.Services;
using BackOnTrack.UI;

namespace BackOnTrack
{
    /// <summary>
    /// Interaction logic for Application.xaml
    /// </summary>
    public partial class Application : Window
    {
        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);

        public UiKeyword UI;
        public ServicesKeyword Services;
        private static Application _instance;
        bool minimizedToTray;
        NotifyIcon trayIcon;

        public Application()
        {
            CheckAndCloseIfApplicationIsAlreadyRunning();

            InitializeComponent();
            _instance = this;
            Setup();
        }

        private void Setup()
        {
            Hide();
            System.Windows.Application.Current.Resources["AccentColor"] = Colors.Teal;

            string[] settings = Environment.GetCommandLineArgs();

            try
            {
                Services = new ServicesKeyword();
                UI = new UiKeyword(!settings.Contains("-startWithoutUi"));
            }
            catch (UnauthorizedAccessException e)
            {
                Messages.CreateMessageBox(
                    "Please make sure you have admin rights and start the application again.",
                    "No admin rights", true);
                Shutdown();
            }
            catch (System.IO.IOException e)
            {
                Messages.CreateMessageBox($"The following error occured with a file:{Environment.NewLine}{Environment.NewLine}{e.Message}", "Error with file", true);
                Shutdown();
            }
        }

        private void CheckAndCloseIfApplicationIsAlreadyRunning()
        {
            Process currentProcess = Process.GetCurrentProcess();
            var runningProcess = (from process in Process.GetProcesses()
                where
                    process.Id != currentProcess.Id &&
                    process.ProcessName.Equals(
                        currentProcess.ProcessName,
                        StringComparison.Ordinal)
                select process).FirstOrDefault();
            if (runningProcess != null)
            {
                ShowWindow(runningProcess.MainWindowHandle, 1);
                Shutdown();
            }
        }

        public static Application Instance()
        {
            return _instance;
        }

        public void Shutdown()
        {
            //here also check if objects are initialized before trying to shut them down
            Environment.Exit(0);
        }

        public void MinimizeToTray()
        {
            trayIcon = new NotifyIcon();
            trayIcon.DoubleClick += new EventHandler(trayIconClick);
            trayIcon.Icon = System.Drawing.Icon.FromHandle(BackOnTrack.Properties.Resources.Icon.Handle);
            trayIcon.Text = "Back on Track";
            trayIcon.Visible = true;

            if (UI.MainView != null)
            {
                UI.MainView.WindowState = WindowState.Minimized;
                UI.MainView.Hide();
            }
            UI.Login.WindowState = WindowState.Minimized;
            UI.Login.Hide();

            minimizedToTray = true;
        }

        private void trayIconClick(Object sender, System.EventArgs e)
        {
            ShowWindow();
        }

        public void ShowWindow()
        {
            if (minimizedToTray)
            {
                trayIcon.Visible = false;
                UI.Login.Show();
                UI.Login.WindowState = WindowState.Normal;
                minimizedToTray = false;
            }
            else
            {
                //Window showWindowToFront;
                //if (UI.MainView != null)
                //{
                //    if(UI.MainView.i)
                //}
                //var window

                WinApi.ShowToFront(new WindowInteropHelper(UI.Login).Handle);
                //muss ueberarbeitet werden
            }
        }

        
    }
}
