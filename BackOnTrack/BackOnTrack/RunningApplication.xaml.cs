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
    /// Interaction logic for RunningApplication.xaml
    /// </summary>
    public partial class RunningApplication : Window
    {
        public UiKeyword UI;
        public ServicesKeyword Services;
        private static RunningApplication _instance;
        bool minimizedToTray;
        NotifyIcon trayIcon;

        public RunningApplication()
        {
            CheckAndCloseIfApplicationIsAlreadyRunning();

            InitializeComponent();
            _instance = this;
            Setup();
            SingleInstance.Stop();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                hwndSource.AddHook(WndProc);
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                ShowWindow();
            }

            return IntPtr.Zero;
        }

        private void Setup()
        {
            Show();//used for onSourceInitialized
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
            if (!SingleInstance.Start())
            {
                SingleInstance.ShowFirstInstance();
                Environment.Exit(0);
                return;
            }
        }

        public static RunningApplication Instance()
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
                Window showWindowToFront = UI.Login;
                if (UI.MainView != null)
                {
                    if (UI.MainView.WindowIsShown)
                    {
                        showWindowToFront = UI.MainView;
                    }
                }

                WinApi.ShowToFront(new WindowInteropHelper(showWindowToFront).Handle);
            }
        }

        
    }
}
