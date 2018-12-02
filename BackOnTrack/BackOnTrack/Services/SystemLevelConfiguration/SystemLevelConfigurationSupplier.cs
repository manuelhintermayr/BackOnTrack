using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOnTrack.Services.SystemLevelConfiguration
{
    public class SystemLevelConfigurationSupplier
    {
        public int CreateNewHostFile()
        {
            return Execute("-createNewHostFile");
        }

        public virtual int Execute(string args)
        {
            string currentLocation = AppDomain.CurrentDomain.BaseDirectory;

            System.Diagnostics.Process execution = new System.Diagnostics.Process();
            execution.StartInfo.FileName = currentLocation+"\\"+ "BackOnTrack.SystemLevelModification.exe";
            execution.StartInfo.Arguments = args;
            execution.Start();
            execution.WaitForExit();
            
            return execution.ExitCode;
        }
    }
}
