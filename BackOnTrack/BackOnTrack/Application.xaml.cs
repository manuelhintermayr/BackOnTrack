using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using BackOnTrack.Services;
using BackOnTrack.UI;
using BackOnTrack.UI.Login;
using BackOnTrack.UI.MainView;

namespace BackOnTrack
{
    /// <summary>
    /// Interaction logic for Application.xaml
    /// </summary>
    public partial class Application : Window
    {
        public UiKeyword UI;
        public ServicesKeyword Services;
        private static Application _instance;

        public Application()
        {
            InitializeComponent();
            _instance = this;
            Setup();
        }

        private void Setup()
        {
            this.Hide();
            Services = new ServicesKeyword();
            UI = new UiKeyword();
        }

        public static Application Instance()
        {
            return _instance;
        }

        public void Shutdown()
        {
            Environment.Exit(0);
        }
    }
}
