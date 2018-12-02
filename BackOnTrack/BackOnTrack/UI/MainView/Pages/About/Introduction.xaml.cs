using System.Windows.Controls;
using System.Windows.Media;

namespace BackOnTrack.UI.MainView.Pages.About
{
    /// <summary>
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class Introduction : UserControl
    {
        private MainView _view;
        public Introduction()
        {
            InitializeComponent();

            //BackOnTrack.Properties.Resources.Logo.

            //LogoImage.Source = (ImageSource)(new ImageSourceConverter().ConvertFrom(BackOnTrack.Properties.Resources.Logo));
        }
    }
}
