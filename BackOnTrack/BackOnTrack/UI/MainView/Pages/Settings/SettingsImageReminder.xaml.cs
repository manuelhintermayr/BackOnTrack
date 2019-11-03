using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using Microsoft.Win32;
using Image = System.Drawing.Image;
using Path = System.IO.Path;

namespace BackOnTrack.UI.MainView.Pages.Settings
{
    /// <summary>
    /// Interaction logic for SettingsImageReminder.xaml
    /// </summary>
    public partial class SettingsImageReminder : UserControl
    {
        private RunningApplication _runningApplication;
        private CurrentProgramConfiguration _configuration;
        private string _newImagePath;
        private string _oldImagePath = $"{RunningApplication.ProgramSettingsPath()}\\.backOnTrack\\imageReminder";

		public SettingsImageReminder()
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
            _configuration = _runningApplication.Services.ProgramConfiguration.TempConfiguration;
            DataContext = _configuration;
            SetNewestImagePath();
        }

        private void SelectImageButton_OnClick(object sender, RoutedEventArgs e)
        {
	        OpenFileDialog open = new OpenFileDialog();
			open.Multiselect = false;
			open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
			if (open.ShowDialog() == true)
			{
				_newImagePath = open.FileName;

				try
				{
					System.Drawing.Image img = System.Drawing.Image.FromFile(_newImagePath);
					ImageDisplayed.Source = null;
					DeleteOldPictures();
					CopyNewPicture();
					SetNewestImagePath();
				}
				catch (Exception err)
				{
					_runningApplication.UI.MainView.CreateAlertWindow("Error with image", $"There was a problem with the image file: " +
					                                                                      $"{Environment.NewLine}{_newImagePath}");
				}
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
	        } else if (FileModification.FileExists($"{_oldImagePath}.jpeg"))
	        {
		        imagePath = $"{_oldImagePath}.jpeg";
			} else if (FileModification.FileExists($"{_oldImagePath}.gif"))
	        {
		        imagePath = $"{_oldImagePath}.gif";
			} else if (FileModification.FileExists($"{_oldImagePath}.bmp"))
	        {
		        imagePath = $"{_oldImagePath}.bmp";
			} else if (FileModification.FileExists($"{_oldImagePath}.png"))
	        {
		        imagePath = $"{_oldImagePath}.png";
			}

	        if (imagePath != "")
	        {
		        try
		        {
			        ImageDisplayed.Source = GetImageByStream(imagePath);
		        }
		        catch (Exception)
		        {
					//image is broken
					ImageDisplayed.Source = null;
		        }
	        }
	        else
	        {
				//image is not in folder
				ImageDisplayed.Source = null;
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

		private void DeleteOldPictures()
        {
	        FileModification.DelteFileIfExists($"{_oldImagePath}.jpg");
			FileModification.DelteFileIfExists($"{_oldImagePath}.jpeg");
			FileModification.DelteFileIfExists($"{_oldImagePath}.gif");
			FileModification.DelteFileIfExists($"{_oldImagePath}.bmp");
			FileModification.DelteFileIfExists($"{_oldImagePath}.png");
		}

        private void CopyNewPicture()
        {
	        string imageExtension = Path.GetExtension(_newImagePath);
	        FileModification.CopyFileFromOnPathToAnother(_newImagePath, $"{_oldImagePath}{imageExtension}");
        }
	}
}
