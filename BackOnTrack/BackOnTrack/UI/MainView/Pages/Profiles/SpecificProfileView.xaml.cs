using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SpecificProfileView.xaml
    /// </summary>
    public partial class SpecificProfileView : UserControl
    {
        private RunningApplication _runningApplication;
        private string _profileName;
        public Profile CurrentProfile { get; set; }
        public static string CurrentSelectedUrlName { get; set; }

        public SpecificProfileView(string profileName)
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
            this.DataContext = this;
            _profileName = profileName;
            Setup(_profileName);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Setup(_profileName);
            EntryList.Items.Refresh();
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

            EntryList.DataContext = CurrentProfile.EntryList;
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
                _runningApplication.UI.MainView.ContentSource = new Uri("/UI/MainView/Pages/Profiles/ViewProfiles.xaml", UriKind.Relative);
            }

        }

        #region Add new Entry

        private void BlockingTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                EntryType newEntryType = (EntryType) e.AddedItems[0];
              
                if (newEntryType == EntryType.Block)
                {
                    RedirectTo.Text = "";
                    RedirectPanel.Visibility = Visibility.Hidden;
                }

                if (newEntryType == EntryType.Redirect)
                {
                    RedirectPanel.Visibility = Visibility.Visible;
                }
            }
            catch (Exception) { }
        }

        private void AddEntryButton_Click(object sender, RoutedEventArgs e)
        {
            EntryType getSelectedEntryType = (EntryType) BlockingTypeComboBox.SelectedItem;
            string addressToBlock = NewAddressToBlockTextbox.Text;
            string addressRedirectTo = RedirectTo.Text;

            if (!AddressValidationRule.IsCorrectAddress(addressToBlock))
            {
                string alertTitle = "Invalid address";
                string alertContent = "The address you entered to block is invalid.";
                _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent);
            }
            else if (AddressValidationRule.IsAddressAlreadyUsed(addressToBlock))
            {
                string alertTitle = "Address already used";
                string alertContent = "The address you entered to block is already used in this or another profile.";
                _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent);
            }
            else
            {
                if (getSelectedEntryType == EntryType.Redirect && !AddressValidationRule.IsCorrectAddress(addressRedirectTo))
                {
                    string alertTitle = "Invalid address";
                    string alertContent = "The address you entered for redirecting is invalid.";
                    _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent);
                }
                else
                {
                    Entry newEntry = null;
                    if (getSelectedEntryType == EntryType.Block)
                    {
                        newEntry = Entry.CreateBlockEntry(addressToBlock);
                    }
                    if (getSelectedEntryType == EntryType.Redirect)
                    {
                        newEntry = Entry.CreateRedirectEntry(addressToBlock, addressRedirectTo);
                    }

                    CurrentProfile.EntryList.Add(newEntry);

                    string alertTitle = "Entry successfully created.";
                    string alertContent = "Entry was successfully created and added.";
                    _runningApplication.UI.MainView.CreateAlertWindow(alertTitle, alertContent);

                    NewAddressToBlockTextbox.Text = "";
                    RedirectTo.Text = "";
                    BlockingTypeComboBox.SelectedIndex = 0;
                    EntryList.Items.Refresh();
                }

            }
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
    }
}
