using System;
using System.IO;
using System.Linq;

namespace DeleteEmptyFolders
{
    public class Methods
    {
        static string GetLastInList(string dirPath)
        {
            string[] allfiles = Directory.GetFileSystemEntries(dirPath, "*.*", SearchOption.TopDirectoryOnly);
            Array.Sort(allfiles);
            return allfiles[allfiles.Length - 1];
        }

        static long GetDirectorySize(string parentDirectory)
        {
            long directorySize = -1;
            try
            {
                directorySize = new DirectoryInfo(parentDirectory).GetFiles("*.*", SearchOption.AllDirectories).Sum(file => file.Length);
            }
            catch (Exception ex)
            {
                Logging.Log("Exeption: " + ex.Message);
            }

            return directorySize;
        }

        public static void DelEmptyFolder(string dirPath, bool lastdirOnly)
        {
            string folder;

            if (lastdirOnly == true)
            {
                folder = Methods.GetLastInList(dirPath);
                Logging.Log("processing folder: " + folder);
            }
            else
            {
                folder = dirPath;
            }

            DelEmptyFolder(folder);
        }

        static void DelEmptyFolder(string dirPath)
        {
            bool isIgnore = false;

            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
            DirectoryInfo[] directoryInfos;

            try
            {
                directoryInfos = directoryInfo.GetDirectories("*", SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            foreach (DirectoryInfo dir in directoryInfos)
            {
                string folderName = dir.FullName;
                long directorySize = Methods.GetDirectorySize(dir.FullName);

                if (folderName.IndexOf("(Full)") > -1 && isIgnore == false)
                {
                    isIgnore = true;
                    Logging.Log($"size = {directorySize},  {dir.FullName} is a Full Backup Folder! => skipped");
                    continue;
                }
                else
                {
                    if (directorySize == 0)
                    {
                        dir.Delete(true);
                        Logging.Log($"size = {directorySize},  {dir.FullName} ==> deleted");
                    }
                    else
                    {
                        Logging.Log($"size = {directorySize},  {dir.FullName} ==> skipped");
                    }
                }
            }

            try
            {
                directoryInfo.Delete();
            }
            catch (Exception ex)
            {
                Logging.Log($"Das Verzeichnis ist nicht leer! ({dirPath})");
            }
        }
    }
}
