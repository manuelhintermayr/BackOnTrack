using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
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
    }
}
