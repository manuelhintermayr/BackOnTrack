using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BackOnTrack.Resources.Infrastructure.Helpers
{
    public static class FileModification
    {
        public static string GetHostFileLocation()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"system32\drivers\etc\hosts");
        }

        public static string ReadFile(string filename)
        {
            string content = "";

            if (FileExists(filename))
            {
                StreamReader fileReader = new StreamReader(filename, System.Text.Encoding.Default);
                content = fileReader.ReadToEnd();
                fileReader.Close();
            }
            return content;
        }

        public static bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        public static void CreateFolderIfNotExists(string foldername)
        {
            DirectoryInfo dir = new DirectoryInfo(foldername);
            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        public static void WriteFile(string filename, string content)
        {
            File.WriteAllText(filename, content);
        }
    }
}
