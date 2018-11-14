using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using BackOnTrack.UI.Login;
using BackOnTrack.UI.MainView;

namespace BackOnTrack
{
    /// <summary>
    /// Interaction logic for Application.xaml
    /// </summary>
    public partial class Application : Window
    {
        public LoginWindow Login;
        public MainView UI;
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
            Login = new LoginWindow();
            Login.Show();
        }

        public static Application Instance()
        {
            return _instance;
        }

        public bool CheckPassword(string password)
        {
            return password == "admin" ? true : false;
        }


        public void Shutdown()
        {
            Environment.Exit(0);
        }
    }
}
