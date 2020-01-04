using System;
using System.IO;
using System.Linq;

namespace DeleteEmptyFolders
{
    public class Methods
    {
        public static void DeleteEmptyLastFolderOnly(DirectoryInfo directoryInfo)
        {
            string folder;
            folder = Methods.GetLastInList(directoryInfo.FullName);
            Logging.Log("processing folder: " + folder);

            DirectoryInfo directory = new DirectoryInfo(folder);
            DeleteEmptyFolders(directory);
        }

        public static void DeleteEmptyFolders(DirectoryInfo directoryInfo)
        {
            DirectoryInfo[] directoryInfos;

            try
            {
                directoryInfos = directoryInfo.GetDirectories("*", SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                Logging.Log(ex.Message);
                return;
            }

            for (int d = directoryInfos.Length - 1; d >= 0; d--)
            {
                string folderName = directoryInfos[d].FullName;
                long directorySize = Methods.GetDirectorySize(directoryInfos[d]);

                if (directorySize == 0)
                {
                    try
                    {
                        if (directoryInfos[d].Attributes.HasFlag(FileAttributes.ReadOnly) == true)
                        {
                            directoryInfos[d].Attributes = FileAttributes.Normal;
                        }
                        directoryInfos[d].Delete();
                        Logging.Log($"empty folder     {directoryInfos[d].FullName} ==> deleted");
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(ex.Message);
                    }
                }
                else
                {
                    string size = directorySize.ToString() + "                 ";
                    Logging.Log($"size = {size.Substring(0, 10)}{folderName} ==> skipped");
                }
            }
        }

        static string GetLastInList(string dirPath)
        {
            string[] allfiles = Directory.GetFileSystemEntries(dirPath, "*.*", SearchOption.TopDirectoryOnly);
            Array.Sort(allfiles);
            return allfiles[allfiles.Length - 1];
        }

        public static long GetDirectorySize(DirectoryInfo path)
        {
            long directorySize = -1;
            try
            {
                directorySize = path.GetFiles("*.*", SearchOption.AllDirectories).Sum(file => file.Length);
            }
            catch (Exception ex)
            {
                Logging.Log("Exeption: " + ex.Message);
            }

            return directorySize;
        }
    }
}
