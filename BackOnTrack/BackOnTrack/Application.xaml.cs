﻿using System;
using System.Windows;
using System.Windows.Media;
using BackOnTrack.Infrastructure.Helpers;
using BackOnTrack.Services;
using BackOnTrack.UI;

namespace BackOnTrack
{
    /// <summary>
    /// Interaction logic for Application.xaml
    /// </summary>
    public partial class Application : Window
    {
        //todo: watch out, that program cannot be opened twice
        //todo: if programm was tried to open twice, open from the running instance the login window
        public UiKeyword UI;
        public ServicesKeyword Services;
        private static Application _instance;

        public Application()
        {
            InitializeComponent();
            _instance = this;
            Setup();
        }

        private void Setup()
        {
            Hide();
            System.Windows.Application.Current.Resources["AccentColor"] = Colors.Teal;
            try
            {
                Services = new ServicesKeyword();
                UI = new UiKeyword();
            }
            catch (UnauthorizedAccessException e)
            {
                Messages.CreateMessageBox(
                    "Please make sure you have admin rights and start the application again.",
                    "No admin rights", true);
                Shutdown();
            }
            catch (System.IO.IOException e)
            {
                Messages.CreateMessageBox($"The following error occured with a file:{Environment.NewLine}{Environment.NewLine}{e.Message}", "Error with file", true);
                Shutdown();
            }
        }

        public static Application Instance()
        {
            return _instance;
        }

        public void Shutdown()
        {
            Environment.Exit(0);
        }
    }
}
