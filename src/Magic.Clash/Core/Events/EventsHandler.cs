using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Core.Events
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

        internal async void ExitHandler()
        {
            try
            {
                await Task.WhenAll(DatabaseManager.Save(ResourcesManager.GetInMemoryLevels())).ConfigureAwait(false);
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
