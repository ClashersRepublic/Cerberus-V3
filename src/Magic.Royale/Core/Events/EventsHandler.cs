using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord;
using Magic.Royale.Core.API.Discord;
using Magic.Royale.Core.Settings;
using Magic.Royale.Logic.Enums;

namespace Magic.Royale.Core.Events
{
    internal class EventsHandler
    {
        internal static EventHandler EHandler;

        internal delegate void EventHandler(Exits Type = Exits.CTRL_CLOSE_EVENT);

        internal EventsHandler()
        {
            EHandler += Handler;
            SetConsoleCtrlHandler(EHandler, true);
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler Handler, bool Enabled);

        internal void ExitHandler()
        {
            try
            {
                if (Constants.UseDiscord)
                    Client.Deinitialize();
                Task.WaitAll(DatabaseManager.Save(ResourcesManager.GetInMemoryLevels()));
            }
            catch (Exception)
            {
                Console.WriteLine("Mmh, something happen when we tried to save everything.");
            }
        }

        internal void Handler(Exits Type = Exits.CTRL_CLOSE_EVENT)
        {
            Logger.SayInfo("The program is shutting down.");
            ExitHandler();
        }
    }
}
