using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.UI.Login;

namespace BackOnTrack.UI
{
    public class UiKeyword
    {
        public LoginWindow Login;
        public MainView.MainView MainView;

        public UiKeyword()
        {
            Login = new LoginWindow();
            Login.Show();
        }

        public void OpenMainView(string password)
        {
            MainView = new MainView.MainView();
            MainView.Show();
        }
    }
}
