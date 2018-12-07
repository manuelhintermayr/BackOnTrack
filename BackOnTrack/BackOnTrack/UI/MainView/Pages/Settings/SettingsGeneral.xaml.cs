using System.Windows.Controls;

namespace BackOnTrack.UI.MainView.Pages.Settings
{
    /// <summary>
    /// Interaction logic for SettingsGeneral.xaml
    /// </summary>
    public partial class SettingsGeneral : UserControl
    {
        private RunningApplication _runningApplication;
        public SettingsGeneral()
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
            DataContext = _runningApplication.Services.ProgramConfiguration.TempConfiguration;
        }
    }
}
