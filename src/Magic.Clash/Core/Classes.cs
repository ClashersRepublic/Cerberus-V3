using System;
using Magic.ClashOfClans.Core.API.Discord;
using Magic.ClashOfClans.Core.Events;
using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Network;
using Magic.Files;

namespace Magic.ClashOfClans.Core
{
    internal static class Classes
    {
        //internal static EventsHandler Events;

        public static void Initialize()
        {
            Z.EntityFramework.Extensions.LicenseManager.AddLicense("1294;103-aididhaiqal02", "d8a78218-810c-f032-ae9d-0f8257e4501c");
            // Initialize our stuff.
            CSV.Initialize();
            Game_Events.Initialize();
            Home.Initialize();
            NPC.Initialize();
            Fingerprint.Initialize();

            ResourcesManager.Initialize();
            ObjectManager.Initialize();


            Logger.Initialize();
            ExceptionLogger.Initialize();

            WebApi.Initialize();
            Gateway.Initialize();
            Timers.Initialize();
            Timers.Run();

            // Start listening since we're done initializing.
            Gateway.Listen();
            if (Constants.UseDiscord)
                Client.Initialize();
            //Events = new EventsHandler();
            Parser.Initialize();
        }
    }
}
