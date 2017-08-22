using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace CRepublic.Restarter
{
    public class Program
    {
        public static Restarter Restarter { get; set; }
        public static string Version { get; set; }

        public static void Main(string[] args)
        {
            var GWL_EXSTYLE = -20;
            var WS_EX_LAYERED = 0x80000;
            uint LWA_ALPHA = 0x2;
            //int LWA_COLORKEY = 0x1;

            // Obtain our handle (hWnd)
            var Handle = GetConsoleWindow();
            SetWindowLong(Handle, GWL_EXSTYLE, GetWindowLong(Handle, GWL_EXSTYLE) ^ WS_EX_LAYERED);
            // Opacity = 0.5 = (255/2)
            SetLayeredWindowAttributes(Handle, 0, 210, LWA_ALPHA);

            if (Console.WindowHeight != 35 || Console.WindowWidth != 130)
                Console.SetWindowSize(130, 35);

            Console.Title = "Clashers' Republic Restarter v" + Assembly.GetExecutingAssembly().GetName().Version +
                            " - Not Running";
            Console.Clear();
            ConsoleUtils.Welcome();

            Version = UpdateChecker.GetVersionString();
            if (Version == Assembly.GetExecutingAssembly().GetName().Version.ToString())
            {
                ConsoleUtils.WriteLineCenterGreen("Restarter is up-to-date! v" +
                                                  Assembly.GetExecutingAssembly().GetName().Version);
            }
            else if (Version == "ConnError")
            {
                ConsoleUtils.WriteLineCenterRed("An error occurred while checking for updates...");
            }
            else
            {
                ConsoleUtils.WriteLineCenterRed("Restarter is not up-to-date! New Version: " +
                                                UpdateChecker.GetVersionString());
                ConsoleUtils.WriteLineCenterYellow("||");
                ConsoleUtils.WriteLineCenterYellow("-> Downloading new version...");
                UpdateChecker.DownloadUpdater();
            }

            var interval = TimeSpan.FromMinutes(30);

            // Make sure we have at least 1 argument.
            if (args.Length < 1)
            {
                ConsoleUtils.WriteLineCenterGreen("||");
                ConsoleUtils.WriteLineCenterGreen("Loaded file path from config: " +
                                                  ConfigurationManager.AppSettings["FileName"]);

                var filePath = ConfigurationManager.AppSettings["FileName"];
                args = new[]
                {
                    filePath
                };
            }

            // Make sure the argument (file) provided exists.
            if (!File.Exists(args[0]))
            {
                ConsoleUtils.WriteLineCenterRed("||");
                ConsoleUtils.WriteLineCenterRed(
                    string.Format("File '{0}' does not exists! Check your 'restarter.config' file and try again.",
                        args[0]));
                ConsoleUtils.WriteLineCenterYellow("Press ENTER to exit...");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

                Environment.Exit(0);
            }

            // Make sure the argument (file) provided is an .exe file.
            if (Path.GetExtension(args[0]) != ".exe")
            {
                ConsoleUtils.WriteLineCenterRed("||");
                ConsoleUtils.WriteLineCenterRed(
                    string.Format("File '{0}' is not a .exe! Check your 'restarter.config' file and try again.",
                        args[0]));
                ConsoleUtils.WriteLineCenterYellow("Press ENTER to exit...");

                while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

                Environment.Exit(0);
            }

            // Make sure the argument (file) provided is not the restarter itself.
            var restarterEXE = Assembly.GetExecutingAssembly().ManifestModule.ToString();

            if (Path.GetFileName(args[0]) == restarterEXE)
            {
                ConsoleUtils.WriteLineCenterRed("||");
                ConsoleUtils.WriteLineCenterRed(
                    $"File '{args[0]}' is the restarter itself! Check your 'restarter.config' file and try again.");
                ConsoleUtils.WriteLineCenterYellow("Press ENTER to exit...");

                while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

                Environment.Exit(0);
            }

            while (true)
            {
                // If we dont have a restart interval provided.
                if (args.Length == 1)
                {
                    ConsoleUtils.WriteLineCenterGreen("||");
                    ConsoleUtils.WriteLineCenterGreen("Loaded restart interval from config: " +
                                                      ConfigurationManager.AppSettings["RestartInterval"]);

                    var intervalStr = ConfigurationManager.AppSettings["RestartInterval"];
                    args = new[]
                    {
                        args[0]
                    };
                }

                if (!TimeSpan.TryParse(ConfigurationManager.AppSettings["RestartInterval"], out interval))
                {
                    ConsoleUtils.WriteLineCenterRed("||");
                    ConsoleUtils.WriteLineCenterRed("Could not parse '" +
                                                    ConfigurationManager.AppSettings["RestartInterval"] +
                                                    "'. Make sure your interval is not higher than 24 hours!");
                    ConsoleUtils.WriteLineCenterYellow(
                        "Press ENTER to exit or SPACE to continue with the default 30 minutes interval...");

                    while (true)
                    {
                        var read = Console.ReadKey();
                        if (read.Key == ConsoleKey.Enter)
                        {
                            Environment.Exit(0);
                        }
                        else if (read.Key == ConsoleKey.Spacebar)
                        {
                            interval = TimeSpan.FromMinutes(30);
                            goto Exit;
                        }
                    }
                }

                Exit:
                break;
            }

            Console.Title = "Clashers' Republic Restarter";

            // Pass argument to the Restarter.
            Restarter = new Restarter(args[0]) {RestartInterval = interval};
            Restarter.Start();

            Thread.Sleep(Timeout.Infinite);
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(int hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool SetLayeredWindowAttributes(int hWnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetWindowLong(int hWnd, int nIndex);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int GetConsoleWindow();
    }
}
