using System;
using System.Collections.Generic;
using System.Threading;
using UCS.Core;
using UCS.Core.Checker;
using UCS.Core.Database;
using UCS.Core.Network;
using UCS.Core.Settings;
using UCS.Core.Threading;
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

            // Wasting main thread because we can right.
            Thread.Sleep(Timeout.Infinite);
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
