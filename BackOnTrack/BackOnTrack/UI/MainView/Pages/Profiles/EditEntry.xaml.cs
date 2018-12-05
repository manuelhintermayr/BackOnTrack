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
using BackOnTrack.Services.UserConfiguration;
using FirstFloor.ModernUI.Windows.Controls;

namespace BackOnTrack.UI.MainView.Pages.Profiles
{
    /// <summary>
    /// Interaction logic for EditEntry.xaml
    /// </summary>
    public partial class EditEntry : UserControl
    {
        private Entry _currentEntry;
        private ModernWindow _window;
        public EditEntry(Entry currentEntry, ModernWindow wnd)
        {
            InitializeComponent();
            _currentEntry = currentEntry;
            _window = wnd;
            Setup();
        }

        public void Setup()
        {
            EntryDomainNameTextBox.Text = _currentEntry.Url;
            EntryBlockingTypeComboBox.SelectedIndex = (int)_currentEntry.EntryType;
            if (_currentEntry.EntryType == EntryType.Redirect)
            {
                EntryRedirectTextBox.Text = _currentEntry.RedirectUrl;
            }
            else
            {
                RedirectStackPanel.Visibility = Visibility.Hidden;
            }
            EntryRunsOnSystemLevelCheckbox.IsChecked = _currentEntry.SystemLevelBlockingIsEnabled;
            EntryRunsOnProxyLevelCheckbox.IsChecked = _currentEntry.ProxyBlockingIsEnabled;
            EntryIsEnabledCheckbox.IsChecked = _currentEntry.IsEnabled;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            //
            _window.Close();
        }
    }
}
