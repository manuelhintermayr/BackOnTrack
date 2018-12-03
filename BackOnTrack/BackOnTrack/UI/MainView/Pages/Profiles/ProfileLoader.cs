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
            return new SpecificProfileView(uri.ToString().Substring(1));
        }
    }
}
