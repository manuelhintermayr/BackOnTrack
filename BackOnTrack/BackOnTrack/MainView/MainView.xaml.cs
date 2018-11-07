using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Controls;

namespace BackOnTrack.MainView
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : ModernWindow
    {
        private MainWindow mainWindow;
        private static MainView instance;
        public MainView(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            Application.Current.Resources["AccentColor"] = Colors.Teal;
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

            mainWindow.OpenLoginView();
        }

        private void ModernWindow_Closed(object sender, EventArgs e)
        {
            mainWindow.Close();
        }

    }
}
