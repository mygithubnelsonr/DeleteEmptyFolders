using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DeleteEmptyFolders
{
    class Program
    {
        /// <summary>
        /// this program expect a valid folder as parameter -path:"foldername" [newest:(true/false)]
        /// </summary>
        /// <param name="args"></param>
        private static string _baseFolder;
        private static bool _onlyLastFolder = false;

        static void Main(string[] args)
        {
            ArgumentHandler(args);

            DeleteJob();

            Console.WriteLine("Press any key...");
            Task.Factory.StartNew(() => Console.ReadKey()).Wait(TimeSpan.FromSeconds(5.0));
        }

        private static void DeleteJob()
        {
            Logging.Log("DeleteEmptyFolders");
            Logging.Log("==================");
            Logging.Log("Base Folder = " + _baseFolder);

            DirectoryInfo directoryInfo = new DirectoryInfo(_baseFolder);

            if (!directoryInfo.Exists)
            {
                Logging.Log("base folder not found!");
                Logging.Log("Exit program.");
                Logging.Flush();
            }
            else if (directoryInfo.FullName.IndexOf("(Full)") > -1)
            {
                long directorySize = Methods.GetDirectorySize(directoryInfo);
                Logging.Log($"size = {directorySize},  {directoryInfo.FullName} is a Full Backup Folder! => skipped");
                Logging.Log("Exit program.");
                Logging.Flush();
            }
            else
            {
                if (_onlyLastFolder == true)
                    Methods.DeleteEmptyLastFolderOnly(directoryInfo);
                else
                    Methods.DeleteEmptyFolders(directoryInfo);
            }

            Logging.Log("finish.");
            Logging.Flush();
        }

        private static void ArgumentHandler(string[] args)
        {
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
                    _baseFolder = arguments[i].Substring(6, arguments[i].Length - 6).ToLower();
                }

                if (loweredArgumentm.StartsWith("-newest:"))
                {
                    string option = arguments[i].Substring(8, arguments[i].Length - 8).ToLower();
                    if (option == "true")
                        _onlyLastFolder = true;
                }
            }
        }
    }
}
