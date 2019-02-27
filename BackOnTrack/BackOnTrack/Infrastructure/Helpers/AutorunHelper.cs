namespace BackOnTrack.Infrastructure.Helpers
{
    public class AutorunHelper
    {
        //got class partly from https://dotnet-snippets.de/snippet/add-remove-registry-entries-for-windows-startup/438
        string runKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private string AppName = "BackOnTrack";
        private Microsoft.Win32.RegistryKey startupKey;

        public bool AddToAutorun()
        {
            if (!AutorunIsEnabled())
            {
                startupKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(runKey, true);
                startupKey.SetValue(AppName, $"\"{System.Reflection.Assembly.GetExecutingAssembly().Location}\" -startWithoutUi");
                startupKey.Close();

                return AutorunIsEnabled();
            }

            return false;
        }

        public bool RemoveFromAutorun()
        {
            if (AutorunIsEnabled())
            {
                startupKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(runKey, true);
                startupKey.DeleteValue(AppName, false);
                startupKey.Close();

                return !AutorunIsEnabled();
            }

            return false;
        }

        public bool AutorunIsEnabled()
        {
            Microsoft.Win32.RegistryKey localStartupKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(runKey);
            bool result = localStartupKey.GetValue(AppName) != null;
            localStartupKey.Close();
            return result;
        }
    }
}
