using System;
using System.Collections.Generic;
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
					_configuration.ImageReminderImageHeight = img.Height.ToString();
					_configuration.ImageReminderImageWidth = img.Width.ToString();
					DeleteOldPictures();
					CopyNewPicture();
				}
				catch (Exception)
				{
					_runningApplication.UI.MainView.CreateAlertWindow("Error with image", $"There was a problem with the image file: " +
					                                                                      $"{Environment.NewLine}{_newImagePath}");
				}
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
