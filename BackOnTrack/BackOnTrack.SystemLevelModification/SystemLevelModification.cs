using System;
using System.IO;
using System.Linq;
using BackOnTrack.SharedResources.Infrastructure.Helpers;


namespace BackOnTrack.SystemLevelModification
{
    public class SystemLevelModification
    {
        private string hostFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"system32\drivers\etc\hosts");
        public string GetHostFileLocation()
        {
            return hostFileLocation; 
        }

        public SystemLevelModification(bool unitTestSetup = false, string newHostFileLocation = "")
        {
            if(newHostFileLocation != "")
            {
                hostFileLocation = newHostFileLocation;
            }

            if (!unitTestSetup)
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
                else if (settings.Contains("-replaceHostFile"))
                {
                    ReplaceHostFile(settings);
                }
                else
                {
                    Environment.Exit(1);
                }
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

        private void ReplaceHostFile(string[] args)
        {
            string newPath = "";

            foreach (var argument in args)
            {
                if (argument.Contains("-newPath"))
                {
                    newPath = argument.Substring(10, (argument.Length-11)).Replace("%20", " ");
                    if (!FileModification.FileExists(newPath))
                    {
                        newPath = "";
                    }
                }
            }

            if(newPath=="")
            {
                Console.WriteLine("No valid path for new system file was given.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            string hostContent = FileModification.ReadFile(newPath);
            FileModification.WriteFile(GetHostFileLocation(), hostContent);
            Environment.Exit(0);

        }

        public bool HostFileExists()
        {
            return FileModification.FileExists(GetHostFileLocation());
        }
    }
}
