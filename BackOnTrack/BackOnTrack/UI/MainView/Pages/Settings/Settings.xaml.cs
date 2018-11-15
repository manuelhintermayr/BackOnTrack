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
using FirstFloor.ModernUI.Windows.Controls;

namespace BackOnTrack.UI.MainView.Pages.Settings
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        private Application _application;

        public Settings()
        {
            _application = Application.Instance();
            InitializeComponent();
        }

        private void ReloadConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ModernDialog
            {
                Title = "Reload Configuration",
                Content = $"This will reset your current program configuration to the current saved configuration. {Environment.NewLine}Are you sure you want to do this?",
                Owner = _application.UI.MainView
                
            };
            dlg.OkButton.Click += new RoutedEventHandler(ResetConfigurationOkClick);
            dlg.Buttons = new Button[] { dlg.OkButton, dlg.CancelButton };
            dlg.ShowDialog();
        }

        private void ResetConfigurationOkClick(object sender, RoutedEventArgs e)
        {
            _application.Services.ProgramConfiguration.SetCurrentConfigurationFromConfig();
        }

        private void SaveConfigurationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RevertChangesButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
