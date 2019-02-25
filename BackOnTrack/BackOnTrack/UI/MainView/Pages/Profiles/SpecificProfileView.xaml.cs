using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BackOnTrack.Infrastructure.Exceptions;
using BackOnTrack.SharedResources.Models;
using FirstFloor.ModernUI.Windows.Controls;

namespace BackOnTrack.UI.MainView.Pages.Profiles
{
    /// <summary>
    /// Interaction logic for SpecificProfileView.xaml
    /// </summary>
    public partial class SpecificProfileView : UserControl
    {
        private RunningApplication _runningApplication;
        private string _profileName;
        public Profile CurrentProfile { get; set; }
        public bool EntryEditButtonIsEnabled {
            get { return (bool)GetValue(EntryEditButtonIsEnabledProperty); }
            set { SetValue(EntryEditButtonIsEnabledProperty, value); }
        }
        
        public static readonly DependencyProperty EntryEditButtonIsEnabledProperty =
            DependencyProperty.Register("EntryEditButtonIsEnabled", typeof(bool), typeof(SpecificProfileView), new PropertyMetadata(false));

        public static string CurrentSelectedUrlName { get; set; }

        public SpecificProfileView(string profileName)
        {
            _runningApplication = RunningApplication.Instance();

            if (!_runningApplication.UnitTestSetup)
            {
                InitializeComponent();
            }

            this.DataContext = this;
            _profileName = profileName;
            Setup(_profileName);
            EntryEditButtonIsEnabled = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Setup(_profileName);
            try
            {
                ProfileIsActivatedCheckbox.IsChecked = CurrentProfile.ProfileIsEnabled;
                EntryList.Items.Refresh();
                EntryEditButtonIsEnabled = true;
            }
            catch (Exception) { }

        }

        private void Setup(string profileName)
        {
            foreach (var profile in _runningApplication.UI.MainView.UserConfiguration.ProfileList)
            {
                
                if (profile.ProfileName == profileName)
                {
                    CurrentProfile = profile;
                    break;
                }
            }

            if (!_runningApplication.UnitTestSetup)
            {
                EntryList.DataContext = CurrentProfile.EntryList;
            }
        }

        private void DeleteCurrentProfile_Click(object sender, RoutedEventArgs e)
        {
            string alertTitle = "Remove profile";
            string alertContent =
                "Are you sure you want to delete this profile? Changes become active after saving.";
            var alertOkEvent = new RoutedEventHandler(DeleteProfileOkClick);

            _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent, true, alertOkEvent);
        }

        private void DeleteProfileOkClick(object sender, RoutedEventArgs e)
        {
            if (!_runningApplication.UI.MainView.UserConfiguration.ProfileList.Remove(CurrentProfile))
            {
                _runningApplication.UI.MainView.CreateAlertWindow("Error removing profile", "Could not remove the profile.");
            }
            else
            {
                ViewProfiles.Instance.UpdateList();
            }

        }

        #region Add new Entry

        private void BlockingTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                EntryType newEntryType = (EntryType) e.AddedItems[0];
              
                if (newEntryType == EntryType.Block || newEntryType == EntryType.RegexBlock)
                {
                    RedirectTo.Text = "";
                    RedirectPanel.Visibility = Visibility.Hidden;
                }

                if (newEntryType == EntryType.Redirect || newEntryType == EntryType.RegexRedirect)
                {
                    RedirectPanel.Visibility = Visibility.Visible;
                }

                if(newEntryType == EntryType.RegexBlock || newEntryType == EntryType.RegexRedirect)
                {
                    NewAddressToBlockText.Content = "Regex to match";
                }
                else
                {
                    NewAddressToBlockText.Content = "Domain/IpAddress";
                }
            }
            catch (Exception) { }
        }

        private void AddEntryButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewEntryFromUi();
        }
        private void NewAddressToBlockTextbox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                AddNewEntryFromUi();
            }
        }

        private void RedirectTo_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                AddNewEntryFromUi();
            }
        }

        private void AddNewEntryFromUi()
        {
            Entry newEntry = GetNewEntryFromInput();

            try
            {
                AddNewEntry(newEntry);

                NewAddressToBlockTextbox.Text = "";
                RedirectTo.Text = "";
                BlockingTypeComboBox.SelectedIndex = 0;
                EntryList.Items.Refresh();
            }
            catch (Exception e)
            {
                _runningApplication.UI.MainView.CreateAlertWindow("Invalid value", e.Message);
            }
        }

        public void AddNewEntry(Entry newEntry)
        {
            CheckIfEntryIsValid(newEntry);
            CurrentProfile.EntryList.Add(newEntry);
        }

        private void CheckIfEntryIsValid(Entry newEntry)
        {
            EntryType getSelectedEntryType = newEntry.EntryType;
            string addressToBlock = newEntry.Url;
            string addressRedirectTo = newEntry.RedirectUrl;

            if (!AddressValidationRule.IsCorrectAddress(addressToBlock)
                && (getSelectedEntryType != EntryType.RegexBlock && getSelectedEntryType != EntryType.RegexRedirect))
            {
                throw new NewEntryException("The address you entered to block is invalid.");
            }
            else if ((getSelectedEntryType == EntryType.RegexBlock || getSelectedEntryType == EntryType.RegexRedirect) &&
                     !AddressValidationRule.IsCorrectRegex(addressToBlock))
            {
                throw new NewEntryException("The regex you entered to match is invalid.");
            }
            else if (AddressValidationRule.IsAddressAlreadyUsed(addressToBlock))
            {
                throw new NewEntryException(
                    "The address you entered to block is already used in this or another profile.");
            }
            else if ((getSelectedEntryType == EntryType.Redirect || getSelectedEntryType == EntryType.RegexRedirect)
                     && !AddressValidationRule.IsCorrectAddress(addressRedirectTo))
            {
                throw new NewEntryException("The address you entered for redirecting is invalid.");
            }
        }

        private Entry GetNewEntryFromInput()
        {
            EntryType getSelectedEntryType = (EntryType)BlockingTypeComboBox.SelectedItem;
            string addressToBlock = NewAddressToBlockTextbox.Text;
            string addressRedirectTo = RedirectTo.Text;

            Entry newEntry = null;
            if (getSelectedEntryType == EntryType.Block)
            {
                newEntry = Entry.CreateBlockEntry(
                    addressToBlock,
                    CurrentProfile.PreferableBlockingOnSystemLevel,
                    CurrentProfile.PreferableBlockingOnProxyLevel
                );
            }
            if (getSelectedEntryType == EntryType.RegexBlock)
            {
                newEntry = Entry.CreateRegexBlockEntry(
                    addressToBlock,
                    CurrentProfile.PreferableBlockingOnProxyLevel
                );
            }
            if (getSelectedEntryType == EntryType.Redirect)
            {
                newEntry = Entry.CreateRedirectEntry(
                    addressToBlock,
                    addressRedirectTo,
                    CurrentProfile.PreferableBlockingOnSystemLevel,
                    CurrentProfile.PreferableBlockingOnProxyLevel
                );
            }
            if (getSelectedEntryType == EntryType.RegexRedirect)
            {
                newEntry = Entry.CreateRegexRedirectEntry(
                    addressToBlock,
                    addressRedirectTo,
                    CurrentProfile.PreferableBlockingOnProxyLevel
                );
            }

            return newEntry;
        }

        #endregion
        #region Remove an Entry

        private void RemoveEntryButton_Click(object sender, RoutedEventArgs e)
        {
            string alertTitle = "Removing entry";
            string alertContent = "Are you sure you want to remove this entry?";
            var alertOkEvent = new RoutedEventHandler(DeleteEntryOkClick);

            _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent, true, alertOkEvent);
        }

        private void DeleteEntryOkClick(object sender, RoutedEventArgs e)
        {
            var entryToRemove = CurrentProfile.EntryList[EntryList.SelectedIndex];

            if (!CurrentProfile.EntryList.Remove(entryToRemove))
            {
                _runningApplication.UI.MainView.CreateAlertWindow("Error removing entry", "Could not remove the entry.");
            }
            else
            {
                EntryList.Items.Refresh();
            }
        }


        #endregion
        #region Edit an existing Entry

        private void EntryEditButton_Click(object sender, RoutedEventArgs e)
        {
            Entry currentEntry = CurrentProfile.EntryList[EntryList.SelectedIndex];

            var wnd = new ModernWindow
            {
                Style = (Style)App.Current.Resources["BlankWindow"],
                Title = $"Edit entry from profile \"{CurrentProfile.ProfileName}\"",
                IsTitleVisible = true,
                Width = 410,
                Height = 400
            };
            wnd.Content = new EditEntry(currentEntry, wnd);
            wnd.ResizeMode = ResizeMode.NoResize;
            wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            wnd.Closing += OnEditWindowClosing;
            wnd.Show();

            _runningApplication.UI.MainView.IsInEntryEditingMode = true;
            _runningApplication.UI.MainView.Hide();
        }

        public void OnEditWindowClosing(object sender, CancelEventArgs e)
        {
            _runningApplication.UI.MainView.Show();
            _runningApplication.UI.MainView.IsInEntryEditingMode = false;
            EntryList.Items.Refresh();
        }

        private void EntryList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string headerName = (string)e.Column.Header;
            if (headerName == "Url")
            {
                CurrentSelectedUrlName = ((Entry) (EntryList.SelectedItem)).Url;
            }
        }

        #endregion

        private void EntryList_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            string headerName = (string)e.Column.Header;
            if (headerName == "Url")
            {
                EntryEditButtonIsEnabled = false;
            }
            
        }

        private void EntryList_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            EntryEditButtonIsEnabled = true;
        }
    }
}
