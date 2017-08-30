using Magic.Royale.Core.API.Discord;
using Magic.Royale.Core.Events;
using Magic.Royale.Core.Settings;
using Magic.Royale.Files;
using Magic.Royale.Network;
using Magic.Files;
using Magic.Royale.Logic.Structure.Game;

namespace Magic.Royale.Core
{
    internal static class Classes
    {
        internal static EventsHandler Events;

        public static void Initialize()
        {
            // Initialize our stuff.
            CSV.Initialize();
            Game_Events.Initialize();
            Deck.Initialize();
            Files.Cards.Initialize();
            Fingerprint.Initialize();

            Arenas.Initialize();
            Rarities.Initialize();
            Logic.Structure.Game.Cards.Initialize();
            Chests.Initialize();

            ResourcesManager.Initialize();
            ObjectManager.Initialize();

            Logger.Initialize();
            ExceptionLogger.Initialize();

            Gateway.Initialize();
            Timers.Initialize();
            Timers.Run();

            // Start listening since we're done initializing.
            Gateway.Listen();
            if (Constants.UseDiscord)
                Client.Initialize();
            Events = new EventsHandler();
        }
    }
}
