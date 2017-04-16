using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UCS.Core;
using UCS.Core.Checker;
using UCS.Core.Database;
using UCS.Core.Network;
using UCS.Core.Settings;
using UCS.Helpers;
using UCS.PacketProcessing.Messages.Client;
using UCS.Utilities.ZLib;

namespace UCS
{
    internal class Program
    {
        public static int OP = 0;

        public static void Main(string[] args)
        {
            // Print welcome message.
            UCSControl.WelcomeMessage();

            // Check directories and files.
            DirectoryChecker.CheckDirectories();
            DirectoryChecker.CheckFiles();

            // Initialize our stuff.
            CSVManager.Initialize();
            ResourcesManager.Initialize();
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
                var clients = ResourcesManager.GetConnectedClients();
                for (int i = 0; i < clients.Count; i++)
                {
                    var client = clients[i];
                    if (DateTime.Now > client.NextKeepAlive)
                    {
                        ResourcesManager.DropClient(client.GetSocketHandle());
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
