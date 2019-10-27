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
using Microsoft.Win32;

namespace BackOnTrack.UI.MainView.Pages.Settings
{
    /// <summary>
    /// Interaction logic for SettingsImageReminder.xaml
    /// </summary>
    public partial class SettingsImageReminder : UserControl
    {
        private RunningApplication _runningApplication;

        public SettingsImageReminder()
        {
            _runningApplication = RunningApplication.Instance();
            InitializeComponent();
            DataContext = _runningApplication.Services.ProgramConfiguration.TempConfiguration;
        }

        private void SelectImageButton_OnClick(object sender, RoutedEventArgs e)
        {
	        OpenFileDialog open = new OpenFileDialog();
			open.Multiselect = false;
			open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
			if (open.ShowDialog() == true)
			{
				String imageFile = open.FileName;
				try
				{
					System.Drawing.Image img = System.Drawing.Image.FromFile(imageFile);
					MessageBox.Show("Width: " + img.Width + ", Height: " + img.Height);
				}
				catch (Exception)
				{
					_runningApplication.UI.MainView.CreateAlertWindow("Error with image", $"There was a problem with the image file: " +
					                                                                      $"{Environment.NewLine}{imageFile}");
				}
			}
		}
    }
}
