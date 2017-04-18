using System;
using System.Net;
using System.IO;
using System.Threading;

namespace Magic.Restarter
{
    internal class UpdateChecker 
    {
        private static WebClient client;

        public static void DownloadUpdater()
        {
            try
            {
                string currentPath = Directory.GetCurrentDirectory();
                string updateURL = "https://www.clashofmagic.net/ucs-restarter.exe";

                client = new WebClient();

                client.DownloadFile(updateURL, currentPath + @"\ucs-restarter_v" + GetVersionString() + ".exe");

                // Sleeping for 100ms just so we don't kill the CPU.
                while (client.IsBusy)
                {
                    Thread.Sleep(100);
                }

                ConsoleUtils.WriteLineCenterGreen("Download finished! Open the new version.");
            }
            catch (Exception)
            {
                ConsoleUtils.WriteLineCenterRed("An error occurred while downloading updates...");
            }
        }

        public static string GetVersionString()
        {
            try
            {
                return new WebClient().DownloadString("https://www.clashofmagic.net/ver.txt");
            }
            catch (Exception)
            {
                return "ConnError";
            }
        }
    }
}