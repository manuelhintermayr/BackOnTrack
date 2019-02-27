using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Interop;
using System.Windows.Media;
using BackOnTrack.Infrastructure.Helpers;
using BackOnTrack.Services;
using BackOnTrack.SharedResources.Infrastructure.Helpers;
using BackOnTrack.UI;

namespace BackOnTrack
{
    /// <summary>
    /// Interaction logic for RunningApplication.xaml
    /// </summary>
    public partial class RunningApplication : Window, IDisposable
    {
        public UiKeyword UI;
        public ServicesKeyword Services;
        private static RunningApplication _instance;
        private static string _programSettingsPath;
        bool _minimizedToTray;
        private NotifyIcon TrayIcon;
        public bool UnitTestSetup = false;
        public bool UiTestSetup = false;

        public RunningApplication()
        {
            ProgramStart(false, "");
        }

        public RunningApplication(bool unitTestSetup = false, string programSettingsPath = "")
        {
            ProgramStart(unitTestSetup, programSettingsPath);
        }

        private void ProgramStart(bool unitTestSetup = false, string programSettingsPath = "")
        {
            UnitTestSetup = unitTestSetup;
            _programSettingsPath = programSettingsPath;
            if (!UnitTestSetup)
            {
                CheckAndCloseIfApplicationIsAlreadyRunning();

                InitializeComponent();
            }

            _instance = this;
            Setup();

            if (!UnitTestSetup)
            {
                SingleInstance.Stop();
            }
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
            //got part from https://www.codeproject.com/Articles/32908/C-Single-Instance-App-With-the-Ability-To-Restore
            if (msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                ShowWindow();
            }

            return IntPtr.Zero;
        }

        private void Setup()
        {
            string[] settings = new string[]{};
            if (!UnitTestSetup)
            {
                Show();//used for onSourceInitialized
                Hide();
                System.Windows.Application.Current.Resources["AccentColor"] = Colors.Teal;
                settings = Environment.GetCommandLineArgs();
                _programSettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }            

            try
            {
                DoUiTestsSetup(settings);

                Services = new ServicesKeyword();
                UI = new UiKeyword(!settings.Contains("-startWithoutUi"));

                if (!UnitTestSetup)
                {
                    if (settings.Contains("-startWithoutUi"))
                    {
                        MinimizeToTray();
                    }
                }
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

        private void DoUiTestsSetup(string[] settings)
        {
            if (!UnitTestSetup)
            {
                if (settings.Contains("-uiTesting"))
                {
                    UiTestSetup = true;

                    foreach (var argument in settings)
                    {
                        if (argument.Contains("-programPath"))
                        {
                            _programSettingsPath = argument.Substring(13, (argument.Length - 13)).Replace("%20", " ");
                            try
                            {
                                FileModification.CreateFolderIfNotExists(_programSettingsPath);
                            }
                            catch (Exception e)
                            {
                                Messages.CreateMessageBox(_programSettingsPath+"="+e.Message, "Error", true);
                            }   
                            FileModification.HostFileLocation = _programSettingsPath + "\\hosts";
                        }
                    }
                }
            }
        }

        private void CheckAndCloseIfApplicationIsAlreadyRunning()
        {
            if (!SingleInstance.Start())
            {
                SingleInstance.ShowFirstInstance();
                Environment.Exit(0);
            }
        }

        public static RunningApplication Instance()
        {
            return _instance;
        }

        public static string ProgramSettingsPath()
        {
            return _programSettingsPath;
        }

        public void Shutdown()
        {
            Services.WebProxy.Dispose();
            //here also check if objects are initialized before trying to shut them down
            Environment.Exit(0);
        }

        public void MinimizeToTray()
        {
            TrayIcon = new NotifyIcon();
            TrayIcon.DoubleClick += new EventHandler(TrayIconClick);
            TrayIcon.Icon = System.Drawing.Icon.FromHandle(BackOnTrack.Properties.Resources.Icon.Handle);
            TrayIcon.Text = "Back on Track";
            TrayIcon.Visible = true;

            if (UI.MainView != null)
            {
                UI.MainView.WindowState = WindowState.Minimized;
                UI.MainView.Hide();
            }
            UI.Login.WindowState = WindowState.Minimized;
            UI.Login.Hide();

            _minimizedToTray = true;
        }

        private void TrayIconClick(Object sender, System.EventArgs e)
        {
            ShowWindow();
        }

        public void ShowWindow()
        {
            if (_minimizedToTray)
            {
                TrayIcon.Visible = false;
                UI.Login.Show();
                UI.Login.WindowState = WindowState.Normal;
                _minimizedToTray = false;
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

                    if (UI.MainView.IsInEntryEditingMode)
                    {
                        return;
                    }
                }

                WinApi.ShowToFront(new WindowInteropHelper(showWindowToFront).Handle);
            }
        }

        public void Dispose()
        {
            Shutdown();
        }
    }
}
