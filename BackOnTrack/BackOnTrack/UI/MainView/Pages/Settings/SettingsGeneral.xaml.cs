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

namespace BackOnTrack.UI.MainView.Pages.Settings
{
    /// <summary>
    /// Interaction logic for SettingsGeneral.xaml
    /// </summary>
    public partial class SettingsGeneral : UserControl
    {
        private RunningApplication _runningApplication;
        public SettingsGeneral()
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
            DataContext = _runningApplication.Services.ProgramConfiguration.TempConfiguration;
        }
    }
}