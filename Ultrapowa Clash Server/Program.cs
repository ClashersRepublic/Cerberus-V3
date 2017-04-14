using System;
using System.Collections.Generic;
using System.Threading;
using UCS.Core;
using UCS.Core.Checker;
using UCS.Core.Database;
using UCS.Core.Network;
using UCS.Core.Settings;
using UCS.Helpers;
using UCS.PacketProcessing.Messages.Client;

namespace UCS
{
    internal class Program
    {
        public static int OP = 0;

        public static void Main(string[] args)
        {
            UCSControl.WelcomeMessage();

            // Check directories and files.
            DirectoryChecker.CheckDirectories();
            DirectoryChecker.CheckFiles();

            // Initialize our stuff.
            ResourcesManager.Initialize();
            CSVManager.Initialize();
            ObjectManager.Initialize();
            Logger.Initialize();
            Gateway.Initialize();

            Gateway.Listen();

            while (true)
            {
                const int SLEEP_TIME = 5000;

                var clients = ResourcesManager.GetConnectedClients();
                for (int i = 0; i < clients.Count; i++)
                {
                    var client = clients[i];
                    if (DateTime.Now > client.NextKeepAlive)
                        ResourcesManager.DropClient(client.GetSocketHandle());
                }

                Thread.Sleep(SLEEP_TIME);
            }
        }

        public static void TitleAd()
        {
            Console.Title = Constants.DefaultTitle + ++OP;
        }

        public static void TitleDe()
        {
            Console.Title = Constants.DefaultTitle + --OP;
        }
    }
}
