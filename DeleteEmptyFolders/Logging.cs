using System;
using System.IO;

namespace DeleteEmptyFolders
{
    public class Logging
    {
        #region Fields
        private static DateTime _dateTime;
        private static string _logname = "action.log";
        private static string _startPath = AppDomain.CurrentDomain.BaseDirectory;
        private static StreamWriter _writer;
        #endregion

        #region CTOR
        static Logging()
        {
            _writer = new StreamWriter(Path.Combine(_startPath, _logname), false);
            _writer.WriteLine(Path.Combine(_startPath, _logname));
        }
        #endregion

        #region Methods
        public static void Flush()
        {
            _writer.Flush();
        }

        public static void Log(string txt)
        {
            _dateTime = DateTime.Now;
            Console.WriteLine(txt);
            _writer.WriteLine("[{0}]  {1}", _dateTime.ToString(), txt);
        }
        #endregion
    }
}
