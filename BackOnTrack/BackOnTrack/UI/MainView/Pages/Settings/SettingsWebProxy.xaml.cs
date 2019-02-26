using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace BackOnTrack.UI.MainView.Pages.Settings
{
    /// <summary>
    /// Interaction logic for SettingsWebProxy.xaml
    /// </summary>
    public partial class SettingsWebProxy : UserControl
    {
        private RunningApplication _runningApplication;
        private string _oldWebProxyPortNumber;

        public SettingsWebProxy()
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
            DataContext = _runningApplication.Services.ProgramConfiguration.TempConfiguration;
        }

        private void ProxyPortAddress_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            _oldWebProxyPortNumber = ProxyPortAddress.Text;
            Regex regex = new Regex("^[0-9]$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void ProxyPortAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newValue = ProxyPortAddress.Text;
            bool newValueIsCorrect = IsValidPortNumber(newValue);

            if (newValue == "")
            {
                _runningApplication.Services.ProgramConfiguration.TempConfiguration.ProxyPortNumber = "";
            }
            else if (!newValueIsCorrect)
            {
                //reset
                _runningApplication.Services.ProgramConfiguration.TempConfiguration.ProxyPortNumber = _oldWebProxyPortNumber;
            }
        }

        public static bool IsValidPortNumber(string portValue)
        {
            Regex regex = new Regex("^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$");
            
            bool newValueIsCorrect = regex.IsMatch(portValue);

            if (portValue.StartsWith("0") && portValue.Length > 1)
            {
                newValueIsCorrect = false;
            }

            return newValueIsCorrect;
        }
    }
}
