using System;
using System.Diagnostics;
using BackOnTrack.Infrastructure.Helpers;
using BackOnTrack.UI.Login;

namespace BackOnTrack.UI
{
    public class UiKeyword
    {
        public LoginWindow Login;
        public MainView.MainView MainView;
        public Application _application;

        public UiKeyword(bool openLoginView = true)
        {
            _application = Application.Instance();
            Login = new LoginWindow();
            if (openLoginView)
            {
                Login.Show();
            }
        }

        public void OpenMainView(string password)
        {
            var userConfiguration = _application.Services.UserConfiguration.OpenConfiguration(password);
            if (userConfiguration == null)
            {
                Messages.CreateMessageBox("User configuration could not be opened.", "Error with user configuration.", true);
            }
            else
            {
                Login.Hide();
                MainView = new MainView.MainView(userConfiguration, password);
                MainView.Show();
            }
        }
    }
}
