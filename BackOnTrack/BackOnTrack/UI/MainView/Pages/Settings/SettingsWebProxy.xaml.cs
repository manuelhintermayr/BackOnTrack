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
using BackOnTrack.Services.ProgramConfiguration;
using BackOnTrack.Services.WebProxy;

namespace BackOnTrack.UI.MainView.Pages.Settings
{
    /// <summary>
    /// Interaction logic for SettingsProxy.xaml
    /// </summary>
    public partial class SettingsProxy : UserControl
    {
        private Application _application;
        public CurrentProgramConfiguration TempConfiguration;
        public SettingsProxy()
        {
            _application = Application.Instance();
            TempConfiguration = _application.Services.ProgramConfiguration.TempConfiguration;
            InitializeComponent();
            DataContext = TempConfiguration;
        }
    }
}
