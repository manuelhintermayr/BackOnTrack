using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.Infrastructure.Helpers;

namespace BackOnTrack.Services.SystemLevelConfiguration
{
    public class SystemLevelConfigurationSupplier
    {
        private string ConfigurationPath;
        public SystemLevelConfigurationSupplier()
        {
            ConfigurationPath =
                $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\.backOnTrack";
            //^todo: make configurable
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
