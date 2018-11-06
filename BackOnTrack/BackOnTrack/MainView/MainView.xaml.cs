using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BackOnTrack.MainView
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private MainWindow mainWindow;
        public MainView(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            //Closing += OnWindowClosing();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainWindow.Close();
        }

    }
}
