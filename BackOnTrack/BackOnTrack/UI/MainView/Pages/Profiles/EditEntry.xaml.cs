using System.Windows;
using System.Windows.Controls;
using BackOnTrack.SharedResources.Models;
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
            EntryAddressTextBox.Text = _currentEntry.Url;
            EntryBlockingTypeComboBox.SelectedIndex = (int)_currentEntry.EntryType;
            if (_currentEntry.EntryType == EntryType.Redirect || _currentEntry.EntryType == EntryType.RegexRedirect)
            {
                EntryRedirectTextBox.Text = _currentEntry.RedirectUrl;
            }
            else
            {
                RedirectStackPanel.Visibility = Visibility.Hidden;
            }
            if(_currentEntry.EntryType == EntryType.RegexBlock || _currentEntry.EntryType == EntryType.RegexRedirect)
            {
                EntryRunsOnSystemLevelCheckbox.IsEnabled = false;
            }
            EntryRunsOnSystemLevelCheckbox.IsChecked = _currentEntry.SystemLevelBlockingIsEnabled;
            EntryRunsOnProxyLevelCheckbox.IsChecked = _currentEntry.ProxyBlockingIsEnabled;
            EntryIsEnabledCheckbox.IsChecked = _currentEntry.IsEnabled;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string addressToBlock = EntryAddressTextBox.Text;
            string addressNameForRedirect = EntryRedirectTextBox.Text;
            bool entryIsEnabled = EntryIsEnabledCheckbox.IsChecked.Value;
            bool entrySystemBlockingIsEnabled = EntryRunsOnSystemLevelCheckbox.IsChecked.Value;
            bool entryProxyBlockingIsEnabled = EntryRunsOnProxyLevelCheckbox.IsChecked.Value;
            var listOfAllAlreadyUsedAddresses = AddressValidationRule.GetListOfAllAlreadyUsedAddresses();
            listOfAllAlreadyUsedAddresses.Remove(_currentEntry.Url);


            if (!AddressValidationRule.IsCorrectAddress(addressToBlock) &&
                (_currentEntry.EntryType != EntryType.RegexBlock && _currentEntry.EntryType != EntryType.RegexRedirect) )
            {
                MakeInvalidValueAlert("New entered address name is invalid.");
            }
            else if (listOfAllAlreadyUsedAddresses.Contains(addressToBlock))
            {
                MakeInvalidValueAlert("New entered address is already used.");
            }
            else if ((_currentEntry.EntryType == EntryType.Redirect || _currentEntry.EntryType == EntryType.RegexRedirect) &&
                     !AddressValidationRule.IsCorrectAddress(addressNameForRedirect))
            {
                MakeInvalidValueAlert("Address for redirect is invalid.");
            }
            else if((_currentEntry.EntryType == EntryType.RegexBlock || _currentEntry.EntryType == EntryType.RegexRedirect) &&
                     !AddressValidationRule.IsCorrectRegex(addressNameForRedirect))
            {
                MakeInvalidValueAlert("Enterd regex is invalid.");
            }
            else
            {
                _currentEntry.Url = addressToBlock;
                _currentEntry.RedirectUrl = (_currentEntry.EntryType == EntryType.Redirect || _currentEntry.EntryType == EntryType.RegexRedirect) ? addressNameForRedirect : "";
                _currentEntry.IsEnabled = entryIsEnabled;
                _currentEntry.SystemLevelBlockingIsEnabled = entrySystemBlockingIsEnabled;
                _currentEntry.ProxyBlockingIsEnabled = entryProxyBlockingIsEnabled;
                _window.Close();
            }
        }

        private void MakeInvalidValueAlert(string alertContent)
        {
            var dlg = new ModernDialog
            {
                Title = "Invalid new value",
                Content = alertContent,
                Owner = _window
            };

            dlg.ShowDialog();
        }
    }
}
