using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool minimizedToTray;
        NotifyIcon notifyIcon;
        private MainUi mainUi;

        public MainWindow()
        {
            if (!SingleInstance.Start())
            {
                SingleInstance.ShowFirstInstance();
                Environment.Exit(0);
                return;
            }

            InitializeComponent();
            this.Hide();
            mainUi = new MainUi(this);
            mainUi.Show();

            SingleInstance.Stop();
        }

        public void Shutdown()
        {
            mainUi.Hide();
            Environment.Exit(0);
        }

    }

}