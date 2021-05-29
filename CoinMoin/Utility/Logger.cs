using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoinMoin.Utility
{
    class Logger
    {
        public string Name { get; set; }
        public int Instance { get; set; }
        public string Path { get; set; }

        public Logger(string _name, int _instance, string _path)
        {
            Name = _name;
            Instance = _instance;
            Path = _path;
        }

        public void Write(LogSeverity severity, string message = "", Exception exception = null)
        {
            using(var sw = new StreamWriter(Path, true))
            {
                if (exception == null)
                    sw.WriteLine($"[{Name}] [{Instance}] [{severity}] [Non-Error]: {message}");
                else
                    sw.WriteLine($"[{Name}] [{Instance}] [{severity}] [Error]: {message} | {exception.Message} | {exception.StackTrace}");
            }
        }

        public enum LogSeverity
        {
            Information,
            Warning,
            Error
        }
    }
}
