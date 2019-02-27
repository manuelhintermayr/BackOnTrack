using System;
using BackOnTrack.SharedResources.Infrastructure.Helpers;

namespace BackOnTrack.Services.SystemLevelConfiguration
{
    public class SystemLevelConfigurationSupplier
    {
        private string ConfigurationPath;
        public SystemLevelConfigurationSupplier()
        {
            ConfigurationPath =
                $"{RunningApplication.ProgramSettingsPath()}\\.backOnTrack";
        }

        public int CreateNewHostFile()
        {
            return Execute("-createNewHostFile");
        }

        public int UpdateHostFile(string hostFileContent)
        {
            string newHostFilePath = $"{ConfigurationPath}\\tempHostFile";
            FileModification.WriteFile(newHostFilePath, hostFileContent);
            return Execute($"-replaceHostFile -newPath='{newHostFilePath.Replace(" ", "%20")}'");

        }

        public virtual int Execute(string args)
        {
            //got part from https://stackoverflow.com/questions/4251694/how-to-start-a-external-executable-from-c-sharp-and-get-the-exit-code-when-the-p
            string currentLocation = AppDomain.CurrentDomain.BaseDirectory;

            System.Diagnostics.Process execution = new System.Diagnostics.Process();
            execution.StartInfo.FileName = currentLocation+"\\"+ "BackOnTrack.SystemLevelModification.exe";
            execution.StartInfo.Arguments = args;
            try
            {
                execution.Start();
                execution.WaitForExit();

                return execution.ExitCode;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return -1;
            }
        }


    }
}
