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

namespace CR.Servers.CoC.Core
{
    internal class Resources
    {
        internal static Accounts Accounts;
        internal static Clans Clans;
        internal static Chats Chats;
        internal static XorShift Random;
        internal static Gateway Gateway;
        internal static Regex Regex;
        internal static bool Started;

        internal static void Initialize()
        {
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

            Resources.Regex = new Regex("[ ]{2,}", RegexOptions.Compiled);

            Resources.Accounts = new Accounts();
            Resources.Clans = new Clans();
            Resources.Chats = new Chats();
            Resources.Random = new XorShift();
            Resources.Gateway = new Gateway();
            Resources.Started = true;

            Parser.Initialize();
            EventsHandler.Initialize();

        }

    }
}
