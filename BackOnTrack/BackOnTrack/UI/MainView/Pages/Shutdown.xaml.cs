using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BackOnTrack.UI.MainView.Pages
{
    /// <summary>
    /// Interaction logic for Shutdown.xaml
    /// </summary>
    public partial class Shutdown : UserControl
    {
        private RunningApplication _runningApplication;

        public Shutdown()
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
        }

        private void ShutdownButton_Click(object sender, RoutedEventArgs e)
        {
            _runningApplication.Shutdown();
        }
    }
}
