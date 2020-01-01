using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DeleteEmptyFolders
{
    class Program
    {
        /// <summary>
        /// this program expect a valid folder as parameter -path:"foldername"
        /// </summary>
        /// <param name="args"></param>
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

            Logging.Log("DeleteEmptyFolders");
            Logging.Log("==================");
            Logging.Log("Base Folder = " + baseFolder);

            DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder);

            if (directoryInfo.Exists)
            {
                Methods.DelEmptyFolder(baseFolder, onlyLastFolder);
                Logging.Log("finish.");
                Logging.Flush();
            }
            else
            {
                Logging.Log("base folder not found!");
                Logging.Log("Exit program.");
                Logging.Flush();
            }

            Console.WriteLine("Press any key...");
            Task.Factory.StartNew(() => Console.ReadKey()).Wait(TimeSpan.FromSeconds(5.0));
        }
    }
}
