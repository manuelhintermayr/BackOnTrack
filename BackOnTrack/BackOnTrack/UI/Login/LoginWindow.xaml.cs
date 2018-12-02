using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using BackOnTrack.Infrastructure.Helpers;

namespace BackOnTrack.UI.Login
{
    internal enum AccentState
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_INVALID_STATE = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    internal enum WindowCompositionAttribute
    {
        // ...
        WCA_ACCENT_POLICY = 19
        // ...
    }

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private MainView.MainView _view;
        private RunningApplication _runningApplication;
        private Brush _correctLoginColor;
        private Brush _wrongLoginColor;
        public const string ConfigurationAlreadyCreated = "Please enter the password you set in the configuration.";
        public const string ConfigurationMustBeCreated = "Please set a password for the configuration.";

        public LoginWindow()
        {
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            _runningApplication = RunningApplication.Instance();
            _correctLoginColor = new SolidColorBrush(Color.FromRgb(51, 51, 51));
            _wrongLoginColor = new SolidColorBrush(Color.FromRgb(255, 117, 117));
            SetConfigurationText();
        }

        public new void Show()
        {
            SetConfigurationText();
            base.Show();
        }

        private void SetConfigurationText()
        {
            ConfigurationText.Text = _runningApplication
                .Services
                .UserConfiguration
                .ConfigurationIsAlreadyCreated()
                ? ConfigurationAlreadyCreated
                : ConfigurationMustBeCreated;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
            MouseLeftButtonDown += delegate { DragMove(); };
            PassworBox.Background = _correctLoginColor;
        }

        private void EnableBlur()
        {
            // got Blur configuration from: https://github.com/riverar/sample-win10-aeroglass
            var windowHelper = new WindowInteropHelper(this);

            var accent = new AccentPolicy();
            var accentStructSize = Marshal.SizeOf(accent);
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        private void Exit_Button_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ExitLoginView();
        }

        private void Login_Button_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ValidateLogin();
        }

        private void PassworBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                ValidateLogin();
            }
        }

        private void PassworBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PassworBox.Background == _wrongLoginColor)
            {
                PassworBox.Background = _correctLoginColor;
            }
        }



        public void ExitLoginView()
        {
            _runningApplication.MinimizeToTray();
        }

        public void ValidateLogin()
        {
            string password = PassworBox.Password;
            bool configurationExists = _runningApplication
                .Services
                .UserConfiguration
                .ConfigurationIsAlreadyCreated();

            if (configurationExists)
            {
                bool passwordCorrect = _runningApplication
                    .Services
                    .UserConfiguration
                    .CheckPassword(password);

                if (passwordCorrect)
                {
                    PassworBox.Password = "";
                    PassworBox.Background = _correctLoginColor;
                    _runningApplication.UI.OpenMainView(password);
                }
                else
                {
                    PassworBox.Background = _wrongLoginColor;
                }
                
            }
            else
            {
                _runningApplication
                    .Services
                    .UserConfiguration
                    .CreateNewConfiguration(password);
                Messages.CreateMessageBox("Created new profile!", "Profile was successfully created.", false);
                PassworBox.Password = "";
                PassworBox.Background = _correctLoginColor;
                _runningApplication.UI.OpenMainView(password);
            }
        }
    }
}
