using System;
using System.IO;

namespace BackOnTrack.SharedResources.Infrastructure.Helpers
{
    public static class FileModification
    {
        public static string HostFileLocation = "";

        public static string GetHostFileLocation()
        {
            if (HostFileLocation == "")
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"system32\drivers\etc\hosts");
            }
            else
            {
                return HostFileLocation;
            }

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

        public static void DelteFileIfExists(string filePath)
        {
	        if (FileExists(filePath))
	        {
				File.Delete(filePath);
	        }
        }

        public static void CopyFileFromOnPathToAnother(string oldFilePath, string newFilePath)
        {
	        File.Copy(oldFilePath, newFilePath);
        }
    }
}
