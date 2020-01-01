using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace DeleteEmptyFolders
{
    class Program
    {
        /// <summary>
        /// this program expect a valid folder as parameter
        /// </summary>
        /// <param name="args"></param>

        #region Fields
        private static DateTime _dateTime;
        private static StreamWriter _writer;
        private static string _logname = "action.log";
        private static string _startPath = AppDomain.CurrentDomain.BaseDirectory;
        #endregion

        static void Main(string[] args)
        {
            #region ArgumentHandler
            string baseFolder = null;
            bool onlyLastFolder = false;
            string[] arguments = Environment.GetCommandLineArgs();

            if (args.Length == 0)
            {
                Console.WriteLine("Parameter is missing!\nExit program.");
                Thread.Sleep(3000);
                return;
            }

            for (int i = 1; i < arguments.Length; i++)
            {
                string loweredArgumentm = arguments[i].ToLower();

                if (loweredArgumentm.IndexOf("?") > -1)
                {
                    Console.WriteLine("usage: DeleteEmptyFolders -path:<start folder> [-newest:true]");
                    Console.WriteLine("press any key ...");
                    Console.ReadKey();
                    return;
                }

                if (loweredArgumentm.StartsWith("-path:"))
                {
                    baseFolder = arguments[i].Substring(6, arguments[i].Length - 6).ToLower();
                }

                if (loweredArgumentm.StartsWith("-newest:"))
                {
                    string option = arguments[i].Substring(8, arguments[i].Length - 8).ToLower();
                    if (option == "true")
                        onlyLastFolder = true;
                }
            }
            #endregion ArgumentHandler


            _writer = new StreamWriter($"{_startPath}\\{_logname}", false);
            //writer = File.AppendText(Path.Combine(startPath, "action.log"));
            _writer.WriteLine(Path.Combine(_startPath, _logname));

            Log("DeleteEmptyFolders");
            Log("==================");
            //Log("Start Path = " + _startPath);
            Log("Base Folder = " + baseFolder);

            DirectoryInfo di = new DirectoryInfo(baseFolder + "x");
            if (di.Exists)
            {
                DelEmptyFolder(baseFolder, onlyLastFolder);
                Log("finish.");
            }
            else
            {
                Log("base folder not found!\nExit program.");
            }

            Thread.Sleep(5000);
        }

        static string GetLastInList(string dirPath)
        {
            string[] allfiles = Directory.GetFileSystemEntries(dirPath, "*.*", SearchOption.TopDirectoryOnly);
            Array.Sort(allfiles);
            return allfiles[allfiles.Length - 1];
        }

        static void DelEmptyFolder(string dirPath)
        {
            DirectoryInfo dirinfo = new DirectoryInfo(dirPath);
            DirectoryInfo[] adirinfo;

            try
            {
                adirinfo = dirinfo.GetDirectories("*", SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            foreach (DirectoryInfo dir in adirinfo)
            {
                long ds = GetDirectorySize(dir.FullName);

                if (ds == 0)
                {
                    dir.Delete(true);
                    Log(String.Format("size = {0},  {1} ==> deleted", ds, dir.FullName));
                }
                else
                {
                    Log(String.Format("size = {0},  {1} ==> deleted", ds, dir.FullName));
                }
            }

            try
            {
                dirinfo.Delete();
            }
            catch
            { }

        }

        static void DelEmptyFolder(string dirPath, bool lastDir)
        {
            string folder;

            if (lastDir == true)
            {
                folder = GetLastInList(dirPath);
                Log("processing folder: " + folder);
            }
            else
            {
                folder = dirPath;
            }


            DelEmptyFolder(folder);
        }

        public static long GetDirectorySize(string parentDirectory)
        {
            long ds = -1;
            try
            {
                ds = new DirectoryInfo(parentDirectory).GetFiles("*.*", SearchOption.AllDirectories).Sum(file => file.Length);
            }
            catch (Exception ex)
            {
                Log("Exeption: " + ex.Message);
            }

            return ds;
        }

        public static void Log(string txt)
        {
            _dateTime = DateTime.Now;
            Console.WriteLine(txt);
            _writer.WriteLine("[{0}]  {1}", _dateTime.ToString(), txt);
        }
    }
}
