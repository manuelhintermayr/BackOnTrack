using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstFloor.ModernUI.Windows;

namespace BackOnTrack.UI.MainView.Pages.Profiles
{
    public class ProfileLoader : DefaultContentLoader
    {
        protected override object LoadContent(Uri uri)
        {
            try
            {
                if (uri.ToString().Contains("/UI/MainView/Pages/Profiles/EmptyView.xaml"))
                {
                    return new EmptyView();
                }

                return new SpecificProfileView(uri.ToString().Substring(1));
            }
            catch (System.InvalidOperationException)
            {
                //happens when specificProfileView is reloaded but old specificProfileView is still in editing mode ==> see todo for that
                string alertTitle = "Could not load profile.";
                string alertContent =
                    "Could not load profile, try opening the page again with the menu. If is doesn`t work, save your changes, log out and back in.";
                RunningApplication.Instance().UI.MainView.CreateAlertWindow(alertTitle, alertContent);

                return new EmptyView();
            }
            
        }
    }
}
