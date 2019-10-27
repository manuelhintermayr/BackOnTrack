using System;
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
using FirstFloor.ModernUI.Windows.Controls;

namespace BackOnTrack.UI.MainView.Pages.Profiles
{
	/// <summary>
	/// Interaction logic for ImageReminderView.xaml
	/// </summary>
	public partial class ImageReminderView : UserControl
	{
		private ModernWindow _window;
		private ViewProfiles _baseView;
		public ImageReminderView(ModernWindow wnd, ViewProfiles viewProfiles)
		{
			InitializeComponent();
			_window = wnd;
			_baseView = viewProfiles;
			Setup();
		}

		private void Setup()
		{
			SaveButton1.IsEnabled = false;
			SaveButton2.IsEnabled = false;
			//SaveButton3.IsEnabled = false;
			SaveButton4.IsEnabled = false;
			SaveButton5.IsEnabled = false;
			SaveButton6.IsEnabled = false;
		}

		private void SaveButton_OnClick(object sender, RoutedEventArgs e)
		{
			_window.Close();
			_baseView.SaveProfiles();
		}

		private void CancelButton_OnClick(object sender, RoutedEventArgs e)
		{
			_window.Close();
		}
	}
}
