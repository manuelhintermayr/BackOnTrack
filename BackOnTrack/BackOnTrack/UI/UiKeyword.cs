using System;
using System.Windows.Forms.VisualStyles;
using BackOnTrack.Infrastructure.Helpers;
using BackOnTrack.UI.Login;

namespace BackOnTrack.UI
{
    public class UiKeyword
    {
        public LoginWindow Login;
        public MainView.MainView MainView;
        private RunningApplication _runningApplication;

        public UiKeyword(bool openLoginView = true)
        {
            _runningApplication = RunningApplication.Instance();
            Login = new LoginWindow();
            if (!_runningApplication.UnitTestSetup)
            {
                if (openLoginView)
                {
                    Login.Show();
                }
            }
        }

        public void OpenMainView(string password, bool showUi = true)
        {
            var userConfiguration = _runningApplication.Services.UserConfiguration.OpenConfiguration(password);
            if (userConfiguration == null)
            {
                string errorMessage = "User configuration could not be opened.";
                string errorTitle = "Error with user configuration.";
                if (showUi)
                {
                    Messages.CreateMessageBox(errorMessage, errorTitle, true);
                }
                else
                {
                    throw new UnauthorizedAccessException(errorMessage);
                }
            }
            else
            {
                if (showUi)
                {
                    Login.Hide();
                }

                MainView = new MainView.MainView(userConfiguration, password);

                if (showUi)
                {
                    MainView.Show();
                }
            }
        }

        public void LoginInMainViewWithoutShowing(string password)
        {
            OpenMainView(password, false);
        }
    }
}
