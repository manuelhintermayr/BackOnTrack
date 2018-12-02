using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOnTrack.SystemLevelModification
{
    public class SystemLevelModification
    {
        public string GetHostFileLocation()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"system32\drivers\etc\hosts");
        }

        public SystemLevelModification()
        {
            Console.WriteLine("Starting BackOnTrackSystemLevelModification");
            Console.WriteLine("Did this window get started by the official BackOnTrack-Application? \"y\"=Yes");
            string result = Console.ReadLine();
            if (result == "y")
            {
                string[] settings = Environment.GetCommandLineArgs();
                if (settings.Length == 1)
                {
                    StartedWithouOptions();
                }
                else if (settings.Contains("-createNewHostFile"))
                {
                    CreateNewHostFile();
                }
                else
                {
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.WriteLine("Closing SystemLevelModification... Please make sure that this tool is only started by the official BackOnTrack application.");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        public void StartedWithouOptions()
        {
            Console.WriteLine("Program was started without options, closing...");
            Console.ReadKey();
            Environment.Exit(1);
        }

        public void CreateNewHostFile()
        {
            if (!HostFileExists())
            {
                try
                {
                    FileModification.WriteFile(GetHostFileLocation(), "#Host-file created by BackOnTrack");
                    Environment.Exit(0);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"The following error occured: [\"{e}\"]{Environment.NewLine}{e.Message}");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.WriteLine($"System file \"{GetHostFileLocation()}\" exists already");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        public bool HostFileExists()
        {
            return FileModification.FileExists(GetHostFileLocation());
        }
    }
}
