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
using BackOnTrack.Services.SystemLevelConfiguration;
using BackOnTrack.SharedResources.Infrastructure.Helpers;
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
		private RunningApplication _runningApplication;
		private CurrentProgramConfiguration _configuration;
		private string _oldImagePath = $"{RunningApplication.ProgramSettingsPath()}\\.backOnTrack\\imageReminder";
		public ImageReminderView(ModernWindow wnd, ViewProfiles viewProfiles)
		{
			_runningApplication = RunningApplication.Instance();
			InitializeComponent();
			_window = wnd;
			_baseView = viewProfiles;
			_configuration = _runningApplication.Services.ProgramConfiguration.TempConfiguration;
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
			SetNewestImagePath();
		}

		private void SetNewestImagePath()
		{
			string imagePath = "";
			if (FileModification.FileExists($"{_oldImagePath}.jpg"))
			{
				imagePath = $"{_oldImagePath}.jpg";
			}
			else if (FileModification.FileExists($"{_oldImagePath}.jpeg"))
			{
				imagePath = $"{_oldImagePath}.jpeg";
			}
			else if (FileModification.FileExists($"{_oldImagePath}.gif"))
			{
				imagePath = $"{_oldImagePath}.gif";
			}
			else if (FileModification.FileExists($"{_oldImagePath}.bmp"))
			{
				imagePath = $"{_oldImagePath}.bmp";
			}
			else if (FileModification.FileExists($"{_oldImagePath}.png"))
			{
				imagePath = $"{_oldImagePath}.png";
			}

			if (imagePath != "")
			{
				try
				{
					System.Drawing.Image img = System.Drawing.Image.FromFile(imagePath);
					var bmp = new BitmapImage();
					bmp.BeginInit();
					bmp.UriSource = new Uri(imagePath);
					bmp.EndInit();

					imageToRemind.Source = bmp;

				}
				catch (Exception)
				{
					//image is broken
					imageToRemind.Source = null;
					_configuration.ImageReminderImageHeight = "0";
					_configuration.ImageReminderImageWidth = "0";
				}
			}
			else
			{
				//image is not in folder
				imageToRemind.Source = null;
				_configuration.ImageReminderImageHeight = "0";
				_configuration.ImageReminderImageWidth = "0";
			}
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
