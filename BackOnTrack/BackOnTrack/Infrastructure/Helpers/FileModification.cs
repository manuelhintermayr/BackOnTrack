using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOnTrack.Infrastructure.Helpers
{
    public static class FileModification
    {
        public static string ReadFile(string filename)
        {
            string content = "";

            if (File.Exists(filename))
            {
                StreamReader file = new StreamReader(filename, System.Text.Encoding.Default);
                content = file.ReadToEnd();
                file.Close();
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
    }
}
