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

namespace BackOnTrack.UI.MainView.Pages.Profiles
{
    /// <summary>
    /// Interaction logic for EditEntry.xaml
    /// </summary>
    public partial class EditEntry : UserControl
    {
        private Entry _currenEntry;
        public EditEntry(Entry currentEntry)
        {
            InitializeComponent();
            _currenEntry = currentEntry;
            Setup();
        }

        public void Setup()
        {
            EntryDomainNameTextBox.Text = _currenEntry.Url;
            EntryBlockingTypeComboBox.SelectedIndex = (int)_currenEntry.EntryType;
            if (_currenEntry.EntryType == EntryType.Redirect)
            {
                EntryRedirectTextBox.Text = _currenEntry.RedirectUrl;
            }
            else
            {
                RedirectStackPanel.Visibility = Visibility.Hidden;
            }
            EntryRunsOnSystemLevelCheckbox.IsChecked = _currenEntry.SystemLevelBlockingIsEnabled;
            EntryRunsOnProxyLevelCheckbox.IsChecked = _currenEntry.ProxyBlockingIsEnabled;
            EntryIsEnabledCheckbox.IsChecked = _currenEntry.IsEnabled;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            //
        }
    }
}
