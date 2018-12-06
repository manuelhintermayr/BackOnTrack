﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Regex regex = new Regex("^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$");
            string newValue = ProxyPortAddress.Text;
            bool newValueIsCorrect = regex.IsMatch(newValue);

            if (newValue.StartsWith("0") && newValue.Length > 1)
            {
                newValueIsCorrect = false;
            }

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
    }
}
