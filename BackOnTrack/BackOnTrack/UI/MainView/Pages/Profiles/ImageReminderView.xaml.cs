using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
		private bool _userCancelledAction;
		private string _oldImagePath = $"{RunningApplication.ProgramSettingsPath()}\\.backOnTrack\\imageReminder";
		public ImageReminderView(ModernWindow wnd, ViewProfiles viewProfiles)
		{
			_runningApplication = RunningApplication.Instance();
			InitializeComponent();
			_userCancelledAction = false;
			_window = wnd;
			_baseView = viewProfiles;
			_configuration = _runningApplication.Services.ProgramConfiguration.TempConfiguration;
			Setup();
		}

		private void Setup()
		{
			SaveButton1.IsEnabled = false;
			SaveButton2.IsEnabled = false;
			SaveButton3.IsEnabled = false;
			SaveButton4.IsEnabled = false;
			SaveButton5.IsEnabled = false;
			SaveButton6.IsEnabled = false;
			SetNewestImagePath();
			StartCounter();
		}

		private void StartCounter()
		{
			Task.Factory.StartNew(() =>
			{
				for (int i = 10; i > 0; i--)
				{
					if (_userCancelledAction)
					{
						break;
					}
					InvokeUI(() => ActiveText.BBCode=$"You still have to wait for {i} seconds.");
					Thread.Sleep(1000);
				}

				if (!_userCancelledAction)
				{
					InvokeUI(() => ActiveText.BBCode = "");
					EnableRandomSaveButton();
				}
			}, TaskCreationOptions.LongRunning);
		}

		private void EnableRandomSaveButton()
		{
			Random r = new Random();
			int rInt = r.Next(1, 6);

			switch (rInt)
			{
				case 1: 
					InvokeUI(() => SaveButton1.IsEnabled = true);
					break;
				case 2:
					InvokeUI(() => SaveButton2.IsEnabled = true);
					break;
				case 3:
					InvokeUI(() => SaveButton3.IsEnabled = true);
					break;
				case 4:
					InvokeUI(() => SaveButton4.IsEnabled = true);
					break;
				case 5:
					InvokeUI(() => SaveButton5.IsEnabled = true);
					break;
				case 6:
				default:
					InvokeUI(() => SaveButton6.IsEnabled = true);
					break;
			}
		}

		private void InvokeUI(Action action)
		{
			Dispatcher.Invoke(action);
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
					imageToRemind.Source = GetImageByStream(imagePath);
				}
				catch (Exception)
				{
					//image is broken
					imageToRemind.Source = null;
				}
			}
			else
			{
				//image is not in folder
				imageToRemind.Source = null;
			}
		}

		private static BitmapImage GetImageByStream(string fileName)
		{
			//got part from https://stackoverflow.com/questions/18167280/image-file-copy-is-being-used-by-another-process
			using (var stream = new FileStream(fileName, FileMode.Open))
			{
				var bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.StreamSource = stream;
				bitmapImage.EndInit();
				bitmapImage.Freeze();
				return bitmapImage;
			}
		}

		private void SaveButton_OnClick(object sender, RoutedEventArgs e)
		{
			_window.Close();
			_baseView.SaveProfiles();
		}

		private void CancelButton_OnClick(object sender, RoutedEventArgs e)
		{
			_userCancelledAction = true;
			_window.Close();
		}
	}
}
