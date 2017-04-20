using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Core.Checker;
using Magic.ClashOfClans.Core.Database;
using Magic.ClashOfClans.Core.Network;
using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans;
using Magic.ClashOfClans.Network.Messages.Client;
using Magic.Utilities.ZLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Magic.ClashOfClans
{
    internal class Program
    {
        public static int OP = 0;

        public static void Main(string[] args)
        {
            // Print welcome message.
            MagicControl.WelcomeMessage();

            // Check directories and files.
            DirectoryChecker.CheckDirectories();
            DirectoryChecker.CheckFiles();

            // Initialize our stuff.
            CsvManager.Initialize();
            ResourcesManagerOld.Initialize();
            ObjectManager.Initialize();

            Logger.Initialize();
            ExceptionLogger.Initialize();

            WebApi.Initialize();
            Gateway.Initialize();

            // Start listening since we're done initializing.
            Gateway.Listen();

            while (true)
            {
                const int SLEEP_TIME = 5000;

                var numDisc = 0;
                var clients = ResourcesManagerOld.GetConnectedClients();
                for (int i = 0; i < clients.Count; i++)
                {
                    var client = clients[i];
                    if (DateTime.Now > client.NextKeepAlive)
                    {
                        ResourcesManagerOld.DropClient(client.GetSocketHandle());
                        numDisc++;
                    }
                }

                if (numDisc > 0)
                    Logger.Say($"Dropped {numDisc} clients due to keep alive timeouts.");

                Thread.Sleep(SLEEP_TIME);
            }
        }

        public static void TitleAd()
        {
            Console.Title = Constants.DefaultTitle + Interlocked.Increment(ref OP);
        }

        public static void TitleDe()
        {
            Console.Title = Constants.DefaultTitle + Interlocked.Decrement(ref OP);
        }
    }
}
