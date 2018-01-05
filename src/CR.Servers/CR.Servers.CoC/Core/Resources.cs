namespace CR.Servers.CoC.Core
{
    using System;
    using System.Text.RegularExpressions;
    using CR.Servers.CoC.Core.Consoles;
    using CR.Servers.CoC.Core.Database;
    using CR.Servers.CoC.Core.Events;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Extensions.Game;
    using CR.Servers.CoC.Files;
    using CR.Servers.CoC.Logic.Chat;
    using CR.Servers.CoC.Logic.Slots;
    using CR.Servers.CoC.Packets;
    using CR.Servers.Core;
    using CR.Servers.Logic.Enums;
    using NLog;

    internal class Resources
    {
        internal static Accounts Accounts;
        internal static Clans Clans;
        internal static Chats Chats;
        internal static XorShift Random;
        internal static Gateway Gateway;
        internal static Regex Regex;
        internal static Regex Name;
        internal static BattlesV2 BattlesV2;
        internal static Timers Timers;
        internal static Test Test;
        internal static Logger Logger;
        internal static PacketManager PacketManager;
        internal static bool Started;
        internal static bool Closing;

        internal static void Initialize()
        {
            Logger = LogManager.GetCurrentClassLogger(typeof(Resources));

            Factory.Initialize();
            CSV.Initialize();
            LevelFile.Initialize();
            Game_Events.Initialize();
            Globals.Initialize();
            Settings.Initialize();
            Fingerprint.Initialize();

            if (Constants.Database == DBMS.Mongo)
            {
                Mongo.Initialize();
            }

            Regex = new Regex("[ ]{2,}", RegexOptions.Compiled);
            Name = new Regex("^[a-zA-Z0-9- ]*$");

            PacketManager = new PacketManager();

            Accounts = new Accounts();
            Clans = new Clans();
            Chats = new Chats();
            BattlesV2 = new BattlesV2();
            Random = new XorShift();
            Gateway = new Gateway();
            Timers = new Timers();
            Started = true;

            Parser.Initialize();
            EventsHandler.Initialize();

#if DEBUG
            Console.WriteLine("We loaded " + Factory.Messages.Count + " messages, " + Factory.Commands.Count + " commands, and " + Factory.Debugs.Count + " debug commands.\n");
            Resources.Test = new Test();
#endif
        }
    }
}