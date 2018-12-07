using System.Windows;
using System.Windows.Controls;

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
