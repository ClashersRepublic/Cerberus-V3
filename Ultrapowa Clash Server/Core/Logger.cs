using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Convert;

namespace UCS.Core
{
    internal static class Logger
    {
        static bool ValidLogLevel;
        static int logLevel = ToInt32(ConfigurationManager.AppSettings["LogLevel"]);
        static string timestamp = Convert.ToString(DateTime.Today).Remove(10).Replace(".", "-").Replace("/", "-");
        static string path = "Logs/log_" + timestamp + "_.txt";
        static SemaphoreSlim _fileLock = new SemaphoreSlim(1);

        private static readonly string _errPath = "Logs/err_" + DateTime.Now.ToFileTime() + "_.log";
        private static readonly StreamWriter _errWriter = new StreamWriter(_errPath);

        public static void Initialize()
        {
            if (logLevel > 2)
            {
                ValidLogLevel = false;
                LogLevelError();
            }
            else
            {
                ValidLogLevel = true;
            }

            if (logLevel != 0 || ValidLogLevel == true)
            {
                if (!File.Exists("Logs/log_" + timestamp + "_.txt"))
                {
                    using (var sw = new StreamWriter("Logs/log_" + timestamp + "_.txt"))
                    {
                        sw.WriteLine("Log file created at " + DateTime.Now);
                        sw.WriteLine();
                    }
                }
            }
        }

        public static async void Write(string text)
        {
            if (logLevel != 0)
            {
                try
                {
                    await _fileLock.WaitAsync();
                    if (logLevel == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[LOG]    " + text);
                        Console.ResetColor();
                    }

                    using (StreamWriter sw = new StreamWriter(path, true))
                        await sw.WriteLineAsync("[LOG]    " + text + " at " + DateTime.UtcNow);
                }
                finally
                {
                    _fileLock.Release();
                }
            }
        }

        public static void Say(string message)
        {
            Console.WriteLine("[UCS]    " + message);
        }

        public static void Say()
        {
            Console.WriteLine();
        }

        public static void Print(string message)
        {
            Console.WriteLine(message);
        }

        public static void Error(string message)
        {
            var text = "[ERROR]  " + message;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();

            _errWriter.WriteLine(text);
        }

        private static void LogLevelError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("Please choose a valid Log Level");
            Console.WriteLine("UCS Emulator is now closing...");
            Console.ResetColor();
            Thread.Sleep(5000);
            Environment.Exit(0);
        }
    }
}
