﻿using System;
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
            EntryAddressTextBox.Text = _currentEntry.Url;
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
            string addressToBlock = EntryAddressTextBox.Text;
            string addressNameForRedirect = EntryRedirectTextBox.Text;
            bool entryIsEnabled = EntryIsEnabledCheckbox.IsChecked.Value;
            bool entrySystemBlockingIsEnabled = EntryRunsOnSystemLevelCheckbox.IsChecked.Value;
            bool entryProxyBlockingIsEnabled = EntryRunsOnProxyLevelCheckbox.IsChecked.Value;
            var listOfAllAlreadyUsedAddresses = AddressValidationRule.GetListOfAllAlreadyUsedAddresses();
            listOfAllAlreadyUsedAddresses.Remove(_currentEntry.Url);


            if (!AddressValidationRule.IsCorrectAddress(addressToBlock))
            {
                MakeInvalidValueAlert("New entered address name is invalid.");
            }
            else if (listOfAllAlreadyUsedAddresses.Contains(addressToBlock))
            {
                MakeInvalidValueAlert("New entered address is already used.");
            }
            else if (_currentEntry.EntryType == EntryType.Redirect &&
                     !AddressValidationRule.IsCorrectAddress(addressNameForRedirect))
            {
                MakeInvalidValueAlert("Address for redirect is invalid.");
            }
            else
            {
                _currentEntry.Url = addressToBlock;
                _currentEntry.RedirectUrl = _currentEntry.EntryType == EntryType.Redirect ? addressNameForRedirect : "";
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