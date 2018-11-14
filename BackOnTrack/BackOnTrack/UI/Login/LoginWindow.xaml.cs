using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace BackOnTrack.UI.Login
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private MainView.MainView view;
        private MainWindow mainWindow;

        public LoginWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
            this.MouseLeftButtonDown += delegate { DragMove(); };
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
            mainWindow.Shutdown();
        }

        private void Login_Button_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //validate login
            HideLoginView();
            view = new MainView.MainView(this);
            view.Show();
        }

        public void OpenLoginView()
        {
            this.Show();
        }

        public void HideLoginView()
        {
            this.Hide();
        }
    }
}
