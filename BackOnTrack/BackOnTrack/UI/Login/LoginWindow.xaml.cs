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

        private MainView.MainView view;
        private Application _application;
        private Brush correctLoginColor;
        private Brush wrongLoginColor;
        public const string configurationAlreadyCreated = "Please enter the password you set in the configuration.";
        public const string configurationMustBeCreated = "Please set a password for the configuration.";

        public LoginWindow()
        {
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            this._application = Application.Instance();
            correctLoginColor = new SolidColorBrush(Color.FromRgb(51, 51, 51));
            wrongLoginColor = new SolidColorBrush(Color.FromRgb(255, 117, 117));
            SetConfigurationText();
        }

        public void Show()
        {
            SetConfigurationText();
            base.Show();
        }

        private void SetConfigurationText()
        {
            this.ConfigurationText.Text = _application
                .Services
                .UserConfiguration
                .ConfigurationIsAlreadyCreated()
                ? configurationAlreadyCreated
                : configurationMustBeCreated;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
            this.MouseLeftButtonDown += delegate { DragMove(); };
            PassworBox.Background = correctLoginColor;
        }

        private void EnableBlur()
        {
            // got Blur configuration from: https://github.com/riverar/sample-win10-aeroglass/blob/master/LICENSE
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
            if (PassworBox.Background == wrongLoginColor)
            {
                PassworBox.Background = correctLoginColor;
            }
        }



        public void ExitLoginView()
        {
            this.Hide();
            _application.Shutdown();
        }

        public void ValidateLogin()
        {
            string password = this.PassworBox.Password;
            bool configurationExists = _application
                .Services
                .UserConfiguration
                .ConfigurationIsAlreadyCreated();

            if (configurationExists)
            {
                bool passwordCorrect = _application
                    .Services
                    .UserConfiguration
                    .CheckPassword(password);

                if (passwordCorrect)
                {
                    PassworBox.Password = "";
                    PassworBox.Background = correctLoginColor;
                    _application.UI.OpenMainView(password);
                }
                else
                {
                    PassworBox.Background = wrongLoginColor;
                }
                
            }
            else
            {
                _application
                    .Services
                    .UserConfiguration
                    .CreateNewConfiguration(password);
                Messages.CreateMessageBox("Created new profile!", "Profile was successfully created.", false);
                PassworBox.Password = "";
                PassworBox.Background = correctLoginColor;
                _application.UI.OpenMainView(password);
            }
        }
    }
}
