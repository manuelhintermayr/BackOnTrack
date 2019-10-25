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
			// open file dialog   
			OpenFileDialog open = new OpenFileDialog();
			open.Multiselect = false;
			// image filters  
			open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
			if (open.ShowDialog() == true)
			{
				String ok = open.SafeFileName;
				//// display image in picture box  
				//pictureBox1.Image = new Bitmap(open.FileName);
				//// image file path  
				//textBox1.Text = open.FileName;
			}
		}
    }
}
