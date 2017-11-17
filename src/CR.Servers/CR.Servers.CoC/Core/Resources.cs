using CR.Servers.CoC.Core.Database;
using CR.Servers.CoC.Core.Events;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Logic.Slots;
using CR.Servers.CoC.Packets;
using CR.Servers.Core;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Core
{
    internal class Resources
    {
        internal static Accounts Accounts;
        internal static XorShift Random;
        internal static Gateway Gateway;
        internal static bool Started;

        internal static void Initialize()
        {
            Factory.Initialize();
            CSV.Initialize();
            LevelFile.Initialize();
            Game_Events.Initialize();

            Globals.Initialize();
            Settings.Initialize();

            if (Constants.Database == DBMS.Mongo)
            {
                Mongo.Initialize();
            }

            Resources.Accounts = new Accounts();
            Resources.Random = new XorShift();
            Resources.Gateway = new Gateway();
            Resources.Started = true;

            EventsHandler.Initialize();

        }

    }
}
